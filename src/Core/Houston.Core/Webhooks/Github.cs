using Houston.Application.WebhookEvents;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models.GitHub.Base;
using Houston.Core.Models.GitHub.Push;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Houston.Application.Webhooks
{
    public class Github : IWebhookService {
		private readonly IHttpContextAccessor _context;

		public Github(IHttpContextAccessor context) {
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public bool RunPipeline(string jsonPayload, List<PipelineTriggerEvent> pipelineTriggerEvents) {
			var @event = _context.HttpContext.Request.Headers["X-GitHub-Event"];

			return (string?)@event switch {
				"push" => HandlePush(jsonPayload, pipelineTriggerEvents),
				_ => false
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

		private static bool HandlePush(string jsonPayload, List<PipelineTriggerEvent> pipelineTriggerEvents) {
			try {
				var payload = JsonSerializer.Deserialize<PushPayload>(jsonPayload);
				if (payload is null)
					return false;

				var paths = new List<string>();

				foreach (var commit in payload.Commits) {
					paths.AddRange(commit.Added);
					paths.AddRange(commit.Modified);
					paths.AddRange(commit.Removed);
				}

				var refSplit = payload.Ref.Split("/");
				var @ref = refSplit[1];
				var refName = string.Join("/", refSplit, 2, refSplit.Length - 2);

				return PushEvent.IsValid(pipelineTriggerEvents, @ref, refName, paths);
			} catch (Exception) {
				return false;
			}
		}
	}
}
