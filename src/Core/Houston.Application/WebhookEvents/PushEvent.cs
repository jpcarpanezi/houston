using Houston.Core.Entities.Postgres;

namespace Houston.Application.WebhookEvents {
	public static class PushEvent {
		public static bool IsValid(List<PipelineTriggerEvent> pipelineTriggerEvent, string @ref, string refName, List<string> path) {
			if (!pipelineTriggerEvent.Any()) {
				return true;
			}

			if (!pipelineTriggerEvent.Any(x => x.TriggerEvent.Value == "push")) {
				return false;
			}

			if (pipelineTriggerEvent.Any(x => x.PipelineTriggerFilters.Any())) {
				var filters = pipelineTriggerEvent.SelectMany(x => x.PipelineTriggerFilters).ToList();

				if (@ref == "heads" && FilterExcludesBranches(filters, refName)) {
					return false;
				}

				if (@ref == "tags" && FilterExcludesTags(filters, refName)) {
					return false;
				}

				if (FilterExcludesPaths(filters, path)) {
					return false;
				}
			}

			return true;
		}

		private static bool FilterExcludesBranches(List<PipelineTriggerFilter> filters, string refName) {
			var branchFilters = filters.Where(x => x.TriggerFilter.Value == "branches");

			return branchFilters.Any() && !branchFilters.SelectMany(x => x.FilterValues).Any(pattern => CheckMatch(pattern, refName));
		}

		private static bool FilterExcludesTags(List<PipelineTriggerFilter> filters, string refName) {
			var tagFilters = filters.Where(x => x.TriggerFilter.Value == "tags");

			return tagFilters.Any() && !tagFilters.SelectMany(x => x.FilterValues).Any(pattern => CheckMatch(pattern, refName));
		}

		private static bool FilterExcludesPaths(List<PipelineTriggerFilter> filters, List<string> path) {
			var pathFilters = filters.Where(x => x.TriggerFilter.Value == "paths");

			return pathFilters.Any() && !pathFilters.SelectMany(x => x.FilterValues).Any(pattern => path.Any(item => CheckMatch(pattern, item)));
		}

		private static bool CheckMatch(string pattern, string item) {
			if (pattern.EndsWith("**")) {
				string patternPrefix = pattern[..^2];

				return item.StartsWith(patternPrefix, StringComparison.OrdinalIgnoreCase);
			}
			
			if (pattern.EndsWith("*")) {
				string patternPrefix = pattern[..^1];

				return item.StartsWith(patternPrefix, StringComparison.OrdinalIgnoreCase) &&
					   !item[patternPrefix.Length..].Contains('/');
			}
			
			return string.Equals(pattern, item, StringComparison.OrdinalIgnoreCase);
		}
	}
}
