namespace Houston.Core.Models {
	public class RunPipelineResult {
		public bool ShouldRun { get; set; }

		public string? Branch { get; set; }

		public RunPipelineResult(bool shouldRun, string? message) {
			ShouldRun = shouldRun;
			Branch = message;
		}
	}
}
