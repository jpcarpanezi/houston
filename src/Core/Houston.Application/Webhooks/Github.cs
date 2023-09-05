using Houston.Application.WebhookEvents;
using Houston.Core.Models.GitHub.Push;

namespace Houston.Application.Webhooks {
	public class Github : IWebhookService {
		private readonly IHttpContextAccessor _context;

		public Github(IHttpContextAccessor context) {
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public RunPipelineResult RunPipeline(string jsonPayload, List<PipelineTriggerEvent> pipelineTriggerEvents) {
			var @event = _context.HttpContext?.Request.Headers["X-GitHub-Event"];

			return (string?)@event switch {
				"push" => HandlePush(jsonPayload, pipelineTriggerEvents),
				_ => new RunPipelineResult(false, null)
			};
		}

		public string? DeserializeOrigin(string jsonPayload) {
			try {
				var payload = JsonSerializer.Deserialize<PushPayload>(jsonPayload);

				return payload?.Repository.SshUrl;
			} catch (Exception) {
				return null;
			}
		}

		private static RunPipelineResult HandlePush(string jsonPayload, List<PipelineTriggerEvent> pipelineTriggerEvents) {
			var result = new RunPipelineResult(false, null);

			try {
				var payload = JsonSerializer.Deserialize<PushPayload>(jsonPayload);
				if (payload is null) return result;

				var paths = new List<string>();

				foreach (var commit in payload.Commits) {
					paths.AddRange(commit.Added);
					paths.AddRange(commit.Modified);
					paths.AddRange(commit.Removed);
				}

				var refSplit = payload.Ref.Split("/");
				var @ref = refSplit[1];
				var refName = string.Join("/", refSplit, 2, refSplit.Length - 2);

				var shouldRun = PushEvent.IsValid(pipelineTriggerEvents, @ref, refName, paths);
				result.Branch = refName;
				result.ShouldRun = true;

				return result;
			} catch (Exception) {
				return result;
			}
		}
	}
}
