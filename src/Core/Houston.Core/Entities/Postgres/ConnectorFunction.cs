namespace Houston.Core.Entities.Postgres;

[Table("ConnectorFunction", Schema = "houston")]
public partial class ConnectorFunction {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("name", TypeName = "character varying")]
	public string Name { get; set; } = null!;

	[Column("description")]
	public string? Description { get; set; }

	[Column("active")]
	public bool Active { get; set; }

	[Column("connector_id")]
	public Guid ConnectorId { get; set; }

	[Column("version", TypeName = "character varying")]
	public string Version { get; set; } = null!;

	[Column("script")]
	public byte[] Script { get; set; } = null!;

	[Column("package")]
	public byte[] Package { get; set; } = null!;

	[Column("build_status")]
	public BuildStatus BuildStatus { get; set; }

	[Column("script_dist")]
	public byte[]? ScriptDist { get; set; } = null!;

	[Column("package_type")]
	public PackageType? PackageType { get; set; }

	[Column("build_stderr")]
	public byte[]? BuildStderr { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(ConnectorId))]
	[InverseProperty(nameof(Postgres.Connector.ConnectorFunction))]
	public virtual Connector Connector { get; set; } = null!;

	[InverseProperty(nameof(ConnectorFunctionInput.ConnectorFunction))]
	public virtual ICollection<ConnectorFunctionInput> ConnectorFunctionInputs { get; set; } = new List<ConnectorFunctionInput>();

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.ConnectorFunctionCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.ConnectorFunctionUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;

	[InverseProperty(nameof(PipelineInstruction.ConnectorFunction))]
	public virtual ICollection<PipelineInstruction> PipelineInstructions { get; } = new List<PipelineInstruction>();
}
