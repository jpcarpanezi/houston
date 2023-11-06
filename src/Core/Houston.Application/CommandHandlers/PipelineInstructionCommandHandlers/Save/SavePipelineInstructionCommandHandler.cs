namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.Save {
	public class SavePipelineInstructionCommandHandler : IRequestHandler<SavePipelineInstructionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public SavePipelineInstructionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(SavePipelineInstructionCommand request, CancellationToken cancellationToken) {
			var orderedPipelineInstructions = request.PipelineInstructions.OrderBy(x => x.ConnectedToArrayIndex);
			var databasePipelineInstructions = await _unitOfWork.PipelineInstructionRepository.GetByPipelineId(request.PipelineId);

			var requestConnectorFunctionHistoryIds = orderedPipelineInstructions.Select(x => x.ConnectorFunctionHistoryId).ToList();
			var databaseConnectorHistoryFunctions = await _unitOfWork.ConnectorFunctionHistoryRepository.GetByIdList(requestConnectorFunctionHistoryIds);

			var instructionsIndexes = orderedPipelineInstructions.Select((el, index) => new { el, index }).ToDictionary(x => x.index, x => Guid.NewGuid());

			var newInstructions = new List<PipelineInstruction>();
			foreach (var instruction in request.PipelineInstructions) {
				var connectorFunction = databaseConnectorHistoryFunctions.FirstOrDefault(x => x.Id == instruction.ConnectorFunctionHistoryId);

				if (connectorFunction is null) {
					return ResultCommand.Conflict("Could not complete request due to a foreign key constraint violation.", "foreignKeyViolation");
				}

				if (connectorFunction.ConnectorFunctionInputs.Count != instruction.Inputs?.Count) {
					return ResultCommand.Forbidden("The number of inserted inputs is not the same as the connector function.", "invalidConnectorFunctionInputs");
				}

				var pipelineInstructionId = instructionsIndexes[request.PipelineInstructions.IndexOf(instruction)];

				var instructionInputs = new List<PipelineInstructionInput>();
				if (instruction.Inputs is not null) {
					instructionInputs = instruction.Inputs.Select(x => new PipelineInstructionInput {
						Id = Guid.NewGuid(),
						InstructionId = pipelineInstructionId,
						InputId = x.Key,
						ReplaceValue = x.Value,
						CreatedBy = _claims.Id,
						CreationDate = DateTime.UtcNow,
						UpdatedBy = _claims.Id,
						LastUpdate = DateTime.UtcNow
					}).ToList();
				}

				var pipelineInstruction = new PipelineInstruction {
					Id = pipelineInstructionId,
					PipelineId = request.PipelineId,
					ConnectorFunctionHistoryId = instruction.ConnectorFunctionHistoryId,
					ConnectedToArrayIndex = instruction.ConnectedToArrayIndex,
					CreatedBy = _claims.Id,
					CreationDate = DateTime.UtcNow,
					UpdatedBy = _claims.Id,
					LastUpdate = DateTime.UtcNow,
					PipelineInstructionInputs = instructionInputs,
				};

				newInstructions.Add(pipelineInstruction);
			}

			_unitOfWork.PipelineInstructionRepository.RemoveRange(databasePipelineInstructions);
			_unitOfWork.PipelineInstructionRepository.AddRange(newInstructions);
			await _unitOfWork.Commit();

			return ResultCommand.Created<List<PipelineInstruction>, List<PipelineInstructionViewModel>>(newInstructions);
		}
	}
}
