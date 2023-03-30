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

	[Column("connection")]
	public Guid? Connection { get; set; }

	[Column("script", TypeName = "character varying[]")]
	public string[] Script { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) without time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) without time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey("Connection")]
	[InverseProperty("InverseConnectionNavigation")]
	public virtual PipelineInstruction? ConnectionNavigation { get; set; }

	[ForeignKey("CreatedBy")]
	[InverseProperty("PipelineInstructionCreatedByNavigation")]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty("ConnectionNavigation")]
	public virtual ICollection<PipelineInstruction> InverseConnectionNavigation { get; } = new List<PipelineInstruction>();

	[ForeignKey("PipelineId")]
	[InverseProperty("PipelineInstruction")]
	public virtual Pipeline Pipeline { get; set; } = null!;

	[InverseProperty("Instruction")]
	public virtual ICollection<PipelineInstructionInput> PipelineInstructionInput { get; } = new List<PipelineInstructionInput>();

	[ForeignKey("UpdatedBy")]
	[InverseProperty("PipelineInstructionUpdatedByNavigation")]
	public virtual User UpdatedByNavigation { get; set; } = null!;

	[InverseProperty("InstructionWithErrorNavigation")]
	public virtual ICollection<PipelineLog> PipelineLogInstructionWithErrorNavigation { get; } = new List<PipelineLog>();
}
