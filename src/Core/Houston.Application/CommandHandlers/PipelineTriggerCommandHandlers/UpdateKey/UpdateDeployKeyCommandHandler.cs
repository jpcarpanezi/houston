namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.UpdateKey {
	public class UpdateDeployKeyCommandHandler : IRequestHandler<UpdateDeployKeyCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdateDeployKeyCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdateDeployKeyCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			var deployKeys = DeployKeysService.Create($"houston-{pipelineTrigger.Id}");
			pipelineTrigger.PrivateKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PrivateKey));
			pipelineTrigger.PublicKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(deployKeys.PublicKey));
			pipelineTrigger.KeyRevealed = false;
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
