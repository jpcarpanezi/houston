namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Get {
	public class GetPipelineTriggerCommandHandler : IRequestHandler<GetPipelineTriggerCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetPipelineTriggerCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetPipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			if (pipelineTrigger is null) {
				return ResultCommand.NotFound("The requested pipeline trigger could not be found.", "pipelineTriggerNotFound");
			}

			return ResultCommand.Ok<PipelineTrigger, PipelineTriggerViewModel>(pipelineTrigger);
		}
	}
}
