namespace Houston.Core.Messages {
	public record RunPipelineMessage(Guid PipelineId, Guid? TriggeredBy, string Branch);
}
