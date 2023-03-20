using EventBus.EventBus.Events;

namespace Houston.Core.Messages {
	public record RunPipelineMessage : IntegrationEvent {
		public string PipelineId { get; set; } = default!;

		public string? TriggeredBy { get; set; }

		public RunPipelineMessage(string pipelineId, string? triggeredBy) {
			PipelineId = pipelineId ?? throw new ArgumentNullException(nameof(pipelineId));
			TriggeredBy = triggeredBy;
		}
	}
}
