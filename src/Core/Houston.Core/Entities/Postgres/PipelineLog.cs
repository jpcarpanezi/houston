namespace Houston.Core.Entities.Postgres {
	public class PipelineLog {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public long ExitCode { get; set; }

		public string? Stdout { get; set; }

		public Guid? TriggeredBy { get; set; }

		public DateTime StartTime { get; set; }

		public TimeOnly Duration { get; set; }

		public byte[] SpecFile { get; set; } = null!;

		public int StepError { get; set; }

		public virtual Pipeline Pipeline { get; set; } = null!;

		public virtual User? TriggeredByNavigation { get; set; }
	}
}
