namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Run {
	public class RunPipelineCommandHandler : IRequestHandler<RunPipelineCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEventBus _eventBus;
		private readonly IUserClaimsService _claims;
		private readonly ILogger<RunPipelineCommandHandler> _logger;

		public RunPipelineCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IUserClaimsService claims, ILogger<RunPipelineCommandHandler> logger) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IResultCommand> Handle(RunPipelineCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return ResultCommand.NotFound("The requested connector could not be found.", "connectorNotFound");
			}

			if (pipeline.Status == PipelineStatusEnum.Running) {
				var avg = await _unitOfWork.PipelineLogsRepository.DurationAverage(request.Id);
				var estimatedCompletionTime = DateTime.UtcNow.AddTicks((long)avg);

				var response = new LockedMessageViewModel("Server is processing a request from this pipeline. Please try again later.", "pipelineRunning", estimatedCompletionTime);
				return ResultCommand.Locked(response);
			}

			try {
				_eventBus.Publish(new RunPipelineMessage(request.Id, _claims.Id));
			} catch (Exception e) {
				_logger.LogError(e, $"Failed to publish {nameof(RunPipelineMessage)}");
				return ResultCommand.InternalServerError("Error while trying to run the pipeline.", "cannotRunPipeline");
			}

			return ResultCommand.NoContent();
		}
	}
}
