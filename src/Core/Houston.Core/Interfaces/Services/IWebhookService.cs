namespace Houston.Core.Interfaces.Services {
	public interface IWebhookService {
		RunPipelineResult RunPipeline(string jsonPayload, List<PipelineTriggerEvent> pipelineTriggerEvents);

		string? DeserializeOrigin(string jsonPayload);
	}
}
