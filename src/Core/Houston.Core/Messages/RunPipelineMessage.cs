using EventBus.EventBus.Events;

namespace Houston.Core.Messages {
	public record RunPipelineMessage : IntegrationEvent {
		public Guid PipelineId { get; set; }

		public Guid? TriggeredBy { get; set; }

		public RunPipelineMessage(Guid pipelineId, Guid? triggeredBy) {
			PipelineId = pipelineId;
			TriggeredBy = triggeredBy;
		}
	}
}
