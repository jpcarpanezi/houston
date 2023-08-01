namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.GetAll {
	public class GetAllPipelineCommandHandler : IRequestHandler<GetAllPipelineCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllPipelineCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllPipelineCommand request, CancellationToken cancellationToken) {
			var connectorFunctions = await _unitOfWork.PipelineRepository.GetAllActives(request.PageSize, request.PageIndex);
			var count = await _unitOfWork.PipelineRepository.CountActives();

			return ResultCommand.Paginated<Pipeline, PipelineViewModel>(connectorFunctions, request.PageSize, request.PageIndex, count);
		}
	}
}
