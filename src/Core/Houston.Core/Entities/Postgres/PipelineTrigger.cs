namespace Houston.Core.Entities.Postgres {
	public class PipelineTrigger {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public string SourceGit { get; set; } = null!;

		public string PrivateKey { get; set; } = null!;

		public string PublicKey { get; set; } = null!;

		public bool KeyRevealed { get; set; }

		public string Secret { get; set; } = null!;

		public Guid CreatedBy { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid UpdatedBy { get; set; }

		public DateTime LastUpdate { get; set; }

		public virtual User CreatedByNavigation { get; set; } = null!;

		public virtual Pipeline Pipeline { get; set; } = null!;

		public virtual User UpdatedByNavigation { get; set; } = null!;
	}
}
