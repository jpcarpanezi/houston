namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.ChangeSecret {
	public class ChangeSecretPipelineTriggerCommandHandler : IRequestHandler<ChangeSecretPipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public ChangeSecretPipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(ChangeSecretPipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByIdAsync(request.PipelineTriggerId);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			pipelineTrigger.Secret = PasswordService.HashPassword(request.NewSecret);
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
