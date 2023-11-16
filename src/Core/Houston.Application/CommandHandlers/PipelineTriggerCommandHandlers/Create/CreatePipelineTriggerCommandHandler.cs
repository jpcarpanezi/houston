namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public class CreatePipelineTriggerCommandHandler : IRequestHandler<CreatePipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreatePipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreatePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var anyPipelineTrigger = await _unitOfWork.PipelineTriggerRepository.AnyPipelineTrigger(request.PipelineId);
			if (anyPipelineTrigger) {
				return ResultCommand.Forbidden("The request pipeline already has a trigger.", "alreadyRegistered");
			}

			var pipelineTriggerId = Guid.NewGuid();
			var deployKeys = DeployKeysService.Create($"houston-{pipelineTriggerId}");

			var pipelineTrigger = new PipelineTrigger {
				Id = pipelineTriggerId,
				PipelineId = request.PipelineId,
				SourceGit = request.SourceGit,
				PrivateKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PrivateKey)),
				PublicKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PublicKey)),
				KeyRevealed = false,
				Secret = PasswordService.HashPassword(request.Secret),
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.PipelineTriggerRepository.Add(pipelineTrigger);
			await _unitOfWork.Commit();

			var response = await _unitOfWork.PipelineTriggerRepository.GetByIdWithInverseProperties(pipelineTriggerId);

			return ResultCommand.Created<PipelineTrigger, PipelineTriggerViewModel>(response!);
		}
	}
}
