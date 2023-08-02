namespace Houston.Core.Interfaces.Services {
	public interface IWebhookService {
		bool RunPipeline(string jsonPayload, List<PipelineTriggerEvent> pipelineTriggerEvents);

		string? DeserializeOrigin(string jsonPayload);
	}
}
