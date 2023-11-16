namespace Houston.Core.Entities.Postgres {
	public class ConnectorFunction {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public bool Active { get; set; }
		
		public string FriendlyName { get; set; } = null!;

		public string Version { get; set; } = null!;

		public byte[] SpecsFile { get; set; } = null!;

		public byte[] Script { get; set; } = null!;

		public byte[] Package { get; set; } = null!;

		public BuildStatus BuildStatus { get; set; }

		public string? BuildStderr { get; set; }

		public byte[]? ScriptDist { get; set; }

		public PackageType? PackageType { get; set; }

		public Guid ConnectorId { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid UpdatedBy { get; set; }

		public DateTime LastUpdate { get; set; }

		public virtual Connector Connector { get; set; } = null!;

		public virtual User CreatedByNavigation { get; set; } = null!;

		public virtual User UpdatedByNavigation { get; set; } = null!;
	}
}
