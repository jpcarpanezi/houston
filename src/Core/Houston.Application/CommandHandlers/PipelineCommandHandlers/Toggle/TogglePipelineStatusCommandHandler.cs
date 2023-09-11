namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Toggle {
	public class TogglePipelineStatusCommandHandler : IRequestHandler<TogglePipelineStatusCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public TogglePipelineStatusCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(TogglePipelineStatusCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return ResultCommand.NotFound("The requested pipeline could not be found.", "pipelineNotFound");
			}

			if (pipeline.Status == PipelineStatus.Running) {
				var avg = await _unitOfWork.PipelineLogsRepository.DurationAverage(request.Id);
				var estimatedCompletionTime = DateTime.UtcNow.AddTicks((long)avg);

				var response = new LockedMessageViewModel("Server is processing a request from this pipeline. Please try again later.", "pipelineRunning", estimatedCompletionTime);
				return ResultCommand.Locked(response);
			}

			pipeline.Status = pipeline.Status == PipelineStatus.Stopped
				? PipelineStatus.Awaiting
				: PipelineStatus.Stopped;
			pipeline.UpdatedBy = _claims.Id;
			pipeline.LastUpdate = DateTime.UtcNow;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
