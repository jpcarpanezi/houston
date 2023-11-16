namespace Houston.Core.Entities.Postgres {
	public class User {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string Password { get; set; } = null!;

		public UserRole Role { get; set; }

		public bool Active { get; set; }

		public bool FirstAccess { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreationDate { get; set; }

		public Guid UpdatedBy { get; set; }

		public DateTime LastUpdate { get; set; }

		public virtual ICollection<Connector> ConnectorCreatedByNavigations { get; set; } = new List<Connector>();

		public virtual ICollection<ConnectorFunction> ConnectorFunctionCreatedByNavigations { get; set; } = new List<ConnectorFunction>();

		public virtual ICollection<ConnectorFunction> ConnectorFunctionUpdatedByNavigations { get; set; } = new List<ConnectorFunction>();

		public virtual ICollection<Connector> ConnectorUpdatedByNavigations { get; set; } = new List<Connector>();

		public virtual User CreatedByNavigation { get; set; } = null!;

		public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

		public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

		public virtual ICollection<Pipeline> PipelineCreatedByNavigations { get; set; } = new List<Pipeline>();

		public virtual ICollection<PipelineLog> PipelineLogs { get; set; } = new List<PipelineLog>();

		public virtual ICollection<PipelineTrigger> PipelineTriggerCreatedByNavigations { get; set; } = new List<PipelineTrigger>();

		public virtual ICollection<PipelineTrigger> PipelineTriggerUpdatedByNavigations { get; set; } = new List<PipelineTrigger>();

		public virtual ICollection<Pipeline> PipelineUpdatedByNavigations { get; set; } = new List<Pipeline>();

		public virtual User UpdatedByNavigation { get; set; } = null!;
	}
}
