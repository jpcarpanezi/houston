using Houston.Core.Commands;
using Houston.Core.Commands.PipelineInstructionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers {
	public class SavePipelineInstructionCommandHandler : IRequestHandler<SavePipelineInstructionCommand, ResultCommand<List<PipelineInstruction>>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public SavePipelineInstructionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<List<PipelineInstruction>>> Handle(SavePipelineInstructionCommand request, CancellationToken cancellationToken) {
			var orderedPipelineInstructions = request.PipelineInstructions.OrderBy(x => x.ConnectedToArrayIndex);
			var databasePipelineInstructions = await _unitOfWork.PipelineInstructionRepository.GetByPipelineId(request.PipelineId);

			var requestConnectorFunctionIds = orderedPipelineInstructions.Select(x => x.ConnectorFunctionId).ToList();
			var databaseConnectorFunctions = await _unitOfWork.ConnectorFunctionRepository.GetByIdList(requestConnectorFunctionIds);

			var instructionsIndexes = orderedPipelineInstructions.Select((el, index) => new { el, index }).ToDictionary(x => x.index, x => Guid.NewGuid());

			var newInstructions = new List<PipelineInstruction>();
			foreach (var instruction in request.PipelineInstructions) {
				var connectorFunction = databaseConnectorFunctions.FirstOrDefault(x => x.Id == instruction.ConnectorFunctionId);

				if (connectorFunction is null) {
					return new ResultCommand<List<PipelineInstruction>>(HttpStatusCode.Conflict, "Could not complete request due to a foreign key constraint violation.", null);
				}

				if (instruction.Inputs is not null && connectorFunction.ConnectorFunctionInputs.Count != instruction.Inputs.Count) {
					return new ResultCommand<List<PipelineInstruction>>(HttpStatusCode.Forbidden, "The number of inserted inputs is not the same as the connector function.", null);
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
					ConnectorFunctionId = instruction.ConnectorFunctionId,
					Connection = instruction.ConnectedToArrayIndex is null ? null : instructionsIndexes[(int)instruction.ConnectedToArrayIndex],
					Script = instruction.Script,
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

			return new ResultCommand<List<PipelineInstruction>>(HttpStatusCode.Created, null, newInstructions);
		}
	}
}
