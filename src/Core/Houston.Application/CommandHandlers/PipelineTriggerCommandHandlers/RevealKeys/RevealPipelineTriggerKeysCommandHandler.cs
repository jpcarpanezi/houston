namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.RevealKeys {
	public class RevealPipelineTriggerKeysCommandHandler : IRequestHandler<RevealPipelineTriggerKeysCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public RevealPipelineTriggerKeysCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService userClaimsService) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = userClaimsService ?? throw new ArgumentNullException(nameof(userClaimsService));
		}

		public async Task<IResultCommand> Handle(RevealPipelineTriggerKeysCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			if (pipelineTrigger.KeyRevealed) {
				return ResultCommand.Forbidden("The deploy keys were already revealed.", "deployKeysRevealed");
			}

			pipelineTrigger.KeyRevealed = true;
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await _unitOfWork.Commit();

			return ResultCommand.Ok<PipelineTrigger, PipelineTriggerKeysViewModel>(pipelineTrigger);
		}
	}
}
