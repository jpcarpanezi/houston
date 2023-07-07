using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("PipelineInstructionInput", Schema = "houston")]
public partial class PipelineInstructionInput {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("input_id")]
	public Guid InputId { get; set; }

	[Column("instruction_id")]
	public Guid InstructionId { get; set; }

	[Column("replace_value")]
	public string? ReplaceValue { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.PipelineInstructionInputCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey(nameof(InputId))]
	[InverseProperty(nameof(Postgres.ConnectorFunctionInput.PipelineInstructionInputs))]
	public virtual ConnectorFunctionInput ConnectorFunctionInput { get; set; } = null!;

	[ForeignKey(nameof(InstructionId))]
	[InverseProperty(nameof(Postgres.PipelineInstruction.PipelineInstructionInputs))]
	public virtual PipelineInstruction PipelineInstruction { get; set; } = null!;

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.PipelineInstructionInputUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
