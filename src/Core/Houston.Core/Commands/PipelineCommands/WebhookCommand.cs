using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class WebhookCommand : IRequest<ResultCommand> {
		public string Origin { get; set; } = null!;

		public Guid PipelineId { get; set; }

		public string JsonPayload { get; set; } = null!;

		public WebhookCommand(string origin, Guid pipelineId, string payload) {
			Origin = origin ?? throw new ArgumentNullException(nameof(origin));
			PipelineId = pipelineId;
			JsonPayload = payload ?? throw new ArgumentNullException(nameof(payload));
		}
	}
}
