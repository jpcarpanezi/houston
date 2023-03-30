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
	public string ReplaceValue { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) without time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) without time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey("CreatedBy")]
	[InverseProperty("PipelineInstructionInputCreatedByNavigation")]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey("InputId")]
	[InverseProperty("PipelineInstructionInput")]
	public virtual ConnectorFunctionInput Input { get; set; } = null!;

	[ForeignKey("InstructionId")]
	[InverseProperty("PipelineInstructionInput")]
	public virtual PipelineInstruction Instruction { get; set; } = null!;

	[ForeignKey("UpdatedBy")]
	[InverseProperty("PipelineInstructionInputUpdatedByNavigation")]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
