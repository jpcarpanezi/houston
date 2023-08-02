namespace Houston.Application.CommandHandlers.PipelineLogCommandHandlers.GetAll {
	public class GetAllPipelineLogCommandHandler : IRequestHandler<GetAllPipelineLogCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllPipelineLogCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllPipelineLogCommand request, CancellationToken cancellationToken) {
			var pipelineLogs = await _unitOfWork.PipelineLogsRepository.GetAllByPipelineId(request.PipelineId, request.PageSize, request.PageIndex);
			var count = await _unitOfWork.PipelineLogsRepository.CountByPipelineId(request.PipelineId);

			return ResultCommand.Paginated<PipelineLog, PipelineLogViewModel>(pipelineLogs, request.PageSize, request.PageIndex, count);
		}
	}
}
