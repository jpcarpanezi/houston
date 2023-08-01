namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Get {
	public class GetPipelineCommandHandler : IRequestHandler<GetPipelineCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetPipelineCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetPipelineCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return ResultCommand.NotFound("The requested pipeline could not be found.", "pipelineNotFound");
			}

			return ResultCommand.Ok<Pipeline, PipelineViewModel>(pipeline);
		}
	}
}
