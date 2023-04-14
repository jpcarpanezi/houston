using EventBus.EventBus.Abstractions;
using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Messages;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class RunPipelineCommandHandler : IRequestHandler<RunPipelineCommand, ResultCommand> {
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

		public async Task<ResultCommand> Handle(RunPipelineCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return new ResultCommand(HttpStatusCode.NotFound, "The requested connector could not be found.", "connectorNotFound");
			}

			if (pipeline.Status == Core.Enums.PipelineStatusEnum.Running) {
				var avg = await _unitOfWork.PipelineLogsRepository.DurationAverage(request.Id);
				return new ResultCommand(HttpStatusCode.Locked, DateTime.UtcNow.AddTicks((long)avg).ToString("yyyy-MM-ddTHH:mm:ssZ"));
			}

			try {
				_eventBus.Publish(new RunPipelineMessage(request.Id, _claims.Id));
			} catch (Exception e) {
				_logger.LogError(e, $"Failed to publish {nameof(RunPipelineMessage)}");
				return new ResultCommand(HttpStatusCode.InternalServerError, "Error while trying to run the pipeline.", "cannotRunPipeline");
			}

			return new ResultCommand(HttpStatusCode.NoContent);
		}
	}
}
