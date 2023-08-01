namespace Houston.Application.CommandHandlers.PipelineLogCommandHandlers.Get {
	public class GetPipelineLogCommandHandler : IRequestHandler<GetPipelineLogCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetPipelineLogCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetPipelineLogCommand request, CancellationToken cancellationToken) {
			var pipelineLog = await _unitOfWork.PipelineLogsRepository.GetByIdWithInverseProperties(request.Id);
			if (pipelineLog is null) {
				return ResultCommand.NotFound("The requested pipeline log could not be found.", "pipelineLogNotFound");
			}

			return ResultCommand.Ok<PipelineLog, PipelineLogViewModel>(pipelineLog);
		}
	}
}
