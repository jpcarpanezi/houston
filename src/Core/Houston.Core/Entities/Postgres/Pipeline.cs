namespace Houston.Core.Entities.Postgres {
	public class Pipeline {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public bool Active { get; set; }

		public PipelineStatus Status { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid UpdatedBy { get; set; }

		public DateTime LastUpdate { get; set; }

		public byte[] SpecFile { get; set; } = null!;

		public virtual User CreatedByNavigation { get; set; } = null!;

		public virtual ICollection<PipelineLog> PipelineLogs { get; set; } = new List<PipelineLog>();

		public virtual PipelineTrigger? PipelineTrigger { get; set; }

		public virtual User UpdatedByNavigation { get; set; } = null!;
	}
}
