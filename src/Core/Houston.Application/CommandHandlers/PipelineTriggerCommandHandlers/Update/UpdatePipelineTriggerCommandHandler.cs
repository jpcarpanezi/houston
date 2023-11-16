namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public class UpdatePipelineTriggerCommandHandler : IRequestHandler<UpdatePipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork __unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdatePipelineTriggerCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			__unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdatePipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await __unitOfWork.PipelineTriggerRepository.GetByIdWithInverseProperties(request.PipelineTriggerId);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			pipelineTrigger.SourceGit = request.SourceGit;
			pipelineTrigger.UpdatedBy = _claims.Id;
			pipelineTrigger.LastUpdate = DateTime.UtcNow;

			__unitOfWork.PipelineTriggerRepository.Update(pipelineTrigger);
			await __unitOfWork.Commit();

			return ResultCommand.Ok<PipelineTrigger, PipelineTriggerViewModel>(pipelineTrigger);
		}
	}
}
