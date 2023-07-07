using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("PipelineInstruction", Schema = "houston")]
public partial class PipelineInstruction {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_id")]
	public Guid PipelineId { get; set; }

	[Column("connector_function_id")]
	public Guid ConnectorFunctionId { get; set; }

	[Column("connection")]
	public Guid? Connection { get; set; }

	[Column("connected_to_array_index")]
	public int? ConnectedToArrayIndex { get; set; }

	[Column("script", TypeName = "character varying[]")]
	public string[] Script { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(Connection))]
	[InverseProperty(nameof(InverseConnectionNavigation))]
	public virtual PipelineInstruction? ConnectionNavigation { get; set; }

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.PipelineInstructionCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty(nameof(ConnectionNavigation))]
	public virtual ICollection<PipelineInstruction> InverseConnectionNavigation { get; } = new List<PipelineInstruction>();

	[ForeignKey(nameof(PipelineId))]
	[InverseProperty(nameof(Postgres.Pipeline.PipelineInstructions))]
	public virtual Pipeline Pipeline { get; set; } = null!;

	[InverseProperty(nameof(PipelineInstructionInput.PipelineInstruction))]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInputs { get; set; } = new List<PipelineInstructionInput>();

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.PipelineInstructionUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;

	[InverseProperty(nameof(PipelineLog.PipelineInstruction))]
	public virtual ICollection<PipelineLog> PipelineLogs { get; } = new List<PipelineLog>();

	[ForeignKey(nameof(ConnectorFunctionId))]
	[InverseProperty(nameof(Postgres.ConnectorFunction.PipelineInstructions))]
	public virtual ConnectorFunction ConnectorFunction { get; set; } = null!;
}
