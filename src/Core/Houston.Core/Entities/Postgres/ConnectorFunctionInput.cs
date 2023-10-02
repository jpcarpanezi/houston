namespace Houston.Core.Entities.Postgres;

[Table("ConnectorFunctionInput", Schema = "houston")]
public partial class ConnectorFunctionInput {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("connector_function_id")]
	public Guid ConnectorFunctionHistoryId { get; set; }

	[Column("name", TypeName = "character varying")]
	public string Name { get; set; } = null!;

	[Column("placeholder", TypeName = "character varying")]
	public string Placeholder { get; set; } = null!;

	[Column("type")]
	public InputType Type { get; set; }

	[Required]
	[Column("required")]
	public bool Required { get; set; }

	[Column("replace", TypeName = "character varying")]
	public string Replace { get; set; } = null!;

	[Column("values", TypeName = "character varying[]")]
	public string[]? Values { get; set; }

	[Column("default_value", TypeName = "character varying")]
	public string? DefaultValue { get; set; }

	[Column("advanced_option")]
	public bool AdvancedOption { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(ConnectorFunctionHistoryId))]
	[InverseProperty(nameof(Postgres.ConnectorFunctionHistory.ConnectorFunctionInputs))]
	public virtual ConnectorFunctionHistory ConnectorFunctionHistory { get; set; } = null!;

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.ConnectorFunctionInputCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty(nameof(PipelineInstructionInput.ConnectorFunctionInput))]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInputs { get; } = new List<PipelineInstructionInput>();

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.ConnectorFunctionInputUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
