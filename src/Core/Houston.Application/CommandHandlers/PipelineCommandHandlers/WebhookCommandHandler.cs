using EventBus.EventBus.Abstractions;
using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Messages;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class WebhookCommandHandler : IRequestHandler<WebhookCommand, ResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEventBus _eventBus;
		private readonly IUserClaimsService _claims;
		private readonly IHttpContextAccessor _context;

		public WebhookCommandHandler(IUnitOfWork unitOfWork, IEventBus eventBus, IUserClaimsService claims, IHttpContextAccessor context) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<ResultCommand> Handle(WebhookCommand request, CancellationToken cancellationToken) {
			var textInfo = new CultureInfo("en-US", false).TextInfo;
			var origin = textInfo.ToTitleCase(request.Origin.ToLower()).Replace(" ", "");

			Type? type = Type.GetType($"Houston.Application.Webhooks.{origin}");
			if (type is null) {
				return new ResultCommand(HttpStatusCode.BadRequest, "The requested webhook origin does not exists.", "invalidWebhookOrigin");
			}

			var webhookService = Activator.CreateInstance(type, _context) as IWebhookService;
			if (webhookService is null) {
				return new ResultCommand(HttpStatusCode.BadRequest, "The requested webhook origin does not exists.", "invalidWebhookOrigin");
			}

			var sourceGit = webhookService.DeserializeOrigin(request.JsonPayload);
			if (sourceGit is null) {
				return new ResultCommand(HttpStatusCode.BadRequest, "The requested webhook origin does not exists.", "invalidWebhookOrigin");
			}

			var triggerEvents = await _unitOfWork.PipelineTriggerRepository.GetByPipelineId(request.PipelineId);
			var pipelineTriggerEvents = triggerEvents?.PipelineTriggerEvents.ToList() ?? new List<PipelineTriggerEvent>();

			var runPipeline = webhookService.RunPipeline(request.JsonPayload, pipelineTriggerEvents);

			if (runPipeline) {
				_eventBus.Publish(new RunPipelineMessage(request.PipelineId, _claims.Id));
			}

			return new ResultCommand(HttpStatusCode.NoContent);
		}
	}
}
