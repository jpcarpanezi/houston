namespace Houston.Core.Entities.Postgres;

[Table("User", Schema = "houston")]
[Index("Email", Name = "User_email_uq", IsUnique = true)]
public partial class User {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("name", TypeName = "character varying")]
	public string Name { get; set; } = null!;

	[Column("email", TypeName = "character varying")]
	public string Email { get; set; } = null!;

	[Column("password")]
	[StringLength(256)]
	public string Password { get; set; } = null!;

	[Column("role")]
	public UserRole Role { get; set; }

	[Required]
	[Column("active")]
	public bool Active { get; set; }

	[Required]
	[Column("first_access")]
	public bool FirstAccess { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[InverseProperty(nameof(Connector.CreatedByNavigation))]
	public virtual ICollection<Connector> ConnectorCreatedByNavigation { get; } = new List<Connector>();

	[InverseProperty(nameof(ConnectorFunction.CreatedByNavigation))]
	public virtual ICollection<ConnectorFunction> ConnectorFunctionCreatedByNavigation { get; } = new List<ConnectorFunction>();

	[InverseProperty(nameof(ConnectorFunctionInput.CreatedByNavigation))]
	public virtual ICollection<ConnectorFunctionInput> ConnectorFunctionInputCreatedByNavigation { get; } = new List<ConnectorFunctionInput>();

	[InverseProperty(nameof(ConnectorFunctionInput.UpdatedByNavigation))]
	public virtual ICollection<ConnectorFunctionInput> ConnectorFunctionInputUpdatedByNavigation { get; } = new List<ConnectorFunctionInput>();

	[InverseProperty(nameof(ConnectorFunction.UpdatedByNavigation))]
	public virtual ICollection<ConnectorFunction> ConnectorFunctionUpdatedByNavigation { get; } = new List<ConnectorFunction>();

	[InverseProperty(nameof(Connector.UpdatedByNavigation))]
	public virtual ICollection<Connector> ConnectorUpdatedByNavigation { get; } = new List<Connector>();

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(InverseCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty(nameof(CreatedByNavigation))]
	public virtual ICollection<User> InverseCreatedByNavigation { get; } = new List<User>();

	[InverseProperty(nameof(UpdatedByNavigation))]
	public virtual ICollection<User> InverseUpdatedByNavigation { get; } = new List<User>();

	[InverseProperty(nameof(Pipeline.CreatedByNavigation))]
	public virtual ICollection<Pipeline> PipelineCreatedByNavigation { get; } = new List<Pipeline>();

	[InverseProperty(nameof(PipelineInstruction.CreatedByNavigation))]
	public virtual ICollection<PipelineInstruction> PipelineInstructionCreatedByNavigation { get; } = new List<PipelineInstruction>();

	[InverseProperty(nameof(PipelineInstructionInput.CreatedByNavigation))]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInputCreatedByNavigation { get; } = new List<PipelineInstructionInput>();

	[InverseProperty(nameof(PipelineInstructionInput.UpdatedByNavigation))]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInputUpdatedByNavigation { get; } = new List<PipelineInstructionInput>();

	[InverseProperty(nameof(PipelineInstruction.UpdatedByNavigation))]
	public virtual ICollection<PipelineInstruction> PipelineInstructionUpdatedByNavigation { get; } = new List<PipelineInstruction>();

	[InverseProperty(nameof(PipelineTrigger.CreatedByNavigation))]
	public virtual ICollection<PipelineTrigger> PipelineTriggerCreatedByNavigation { get; } = new List<PipelineTrigger>();

	[InverseProperty(nameof(PipelineTrigger.UpdatedByNavigation))]
	public virtual ICollection<PipelineTrigger> PipelineTriggerUpdatedByNavigation { get; } = new List<PipelineTrigger>();

	[InverseProperty(nameof(Pipeline.UpdatedByNavigation))]
	public virtual ICollection<Pipeline> PipelineUpdatedByNavigation { get; } = new List<Pipeline>();

	[InverseProperty(nameof(PipelineLog.TriggeredByNavigation))]
	public virtual ICollection<PipelineLog> PipelineLogTriggeredByNavigation { get; } = new List<PipelineLog>();

	[InverseProperty(nameof(ConnectorFunctionHistory.CreatedByNavigation))]
	public virtual ICollection<ConnectorFunctionHistory> ConnectorFunctionHistoryCreatedByNavigation { get; } = new List<ConnectorFunctionHistory>();

	[InverseProperty(nameof(ConnectorFunctionHistory.UpdatedByNavigation))]
	public virtual ICollection<ConnectorFunctionHistory> ConnectorFunctionHistoryUpdatedByNavigation { get; } = new List<ConnectorFunctionHistory>();

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(InverseUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
