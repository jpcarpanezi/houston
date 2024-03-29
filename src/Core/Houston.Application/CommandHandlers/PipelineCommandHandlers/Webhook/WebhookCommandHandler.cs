﻿namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Webhook {
	public class WebhookCommandHandler : IRequestHandler<WebhookCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPublishEndpoint _eventBus;
		private readonly IHttpContextAccessor _context;

		public WebhookCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint eventBus, IHttpContextAccessor context) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IResultCommand> Handle(WebhookCommand request, CancellationToken cancellationToken) {
			var textInfo = new CultureInfo("en-US", false).TextInfo;
			var origin = textInfo.ToTitleCase(request.Origin.ToLower()).Replace(" ", "");

			Type? type = Type.GetType($"Houston.Application.Webhooks.{origin}");
			if (type is null) {
				return ResultCommand.NotFound("The requested webhook origin does not exists.", "invalidWebhookOrigin");
			}

			if (Activator.CreateInstance(type, _context) is not IWebhookService webhookService) {
				return ResultCommand.NotFound("The requested webhook origin does not exists.", "invalidWebhookOrigin");
			}

			var sourceGit = webhookService.DeserializeOrigin(request.JsonPayload);
			if (sourceGit is null) {
				return ResultCommand.NotFound("The requested webhook origin does not exists.", "invalidWebhookOrigin");
			}

			var triggerEvents = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			var pipelineTriggerEvents = triggerEvents?.PipelineTriggerEvents.ToList() ?? new List<PipelineTriggerEvent>();

			var runPipeline = webhookService.RunPipeline(request.JsonPayload, pipelineTriggerEvents);

			if (runPipeline.ShouldRun) {
				await _eventBus.Publish(new RunPipelineMessage(request.PipelineId, null, runPipeline.Branch!), cancellationToken);
			}

			return ResultCommand.NoContent();
		}
	}
}
