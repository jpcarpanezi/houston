namespace Houston.Core.Entities.Postgres;

[Table("ConnectorFunctionHistory", Schema = "houston")]
[Index("Version", Name = "ConnectorFunctionHistory_version_index", IsUnique = false)]
public class ConnectorFunctionHistory {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("connector_function_id")]
	public Guid ConnectorFunctionId { get; set; }

	[Column("active")]
	public bool Active { get; set; }

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
	public byte[]? BuildStderr { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(ConnectorFunctionId))]
	[InverseProperty(nameof(Postgres.ConnectorFunction.ConnectorFunctionHistories))]
	public virtual ConnectorFunction ConnectorFunction { get; set; } = null!;

	[InverseProperty(nameof(ConnectorFunctionInput.ConnectorFunctionHistory))]
	public virtual ICollection<ConnectorFunctionInput> ConnectorFunctionInputs { get; set; } = new List<ConnectorFunctionInput>();

	[InverseProperty(nameof(PipelineInstruction.ConnectorFunctionHistory))]
	public virtual ICollection<PipelineInstruction> PipelineInstructions { get; set; } = new List<PipelineInstruction>();

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.ConnectorFunctionHistoryCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.ConnectorFunctionHistoryUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
