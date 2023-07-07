namespace Houston.Application.ViewModel.PipelineLogViewModels {
	public class PipelineLogViewModel {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public int ExitCode { get; set; }

		public long RunN { get; set; }

		public string? Stdout { get; set; }

		public Guid? InstructionWithErrorId { get; set; }

		public string InstructionWithError { get; set; } = null!;

		public Guid? TriggeredById { get; set; }

		public string TriggeredBy { get; set; } = null!;

		public DateTime StartTime { get; set; }

		public TimeSpan Duration { get; set; }
	}
}
