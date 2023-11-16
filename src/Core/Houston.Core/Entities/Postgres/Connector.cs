namespace Houston.Core.Entities.Postgres {
	public class Connector {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public bool Active { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid UpdatedBy { get; set; }

		public DateTime LastUpdate { get; set; }

		public string FriendlyName { get; set; } = null!;

		public virtual ICollection<ConnectorFunction> ConnectorFunctions { get; set; } = new List<ConnectorFunction>();

		public virtual User CreatedByNavigation { get; set; } = null!;

		public virtual User UpdatedByNavigation { get; set; } = null!;
	}
}
