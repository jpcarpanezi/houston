using Houston.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("Pipeline", Schema = "houston")]
public partial class Pipeline {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("name", TypeName = "character varying")]
	public string Name { get; set; } = null!;

	[Column("description", TypeName = "character varying")]
	public string? Description { get; set; }

	[Column("active")]
	public bool Active { get; set; }

	[Column("status")]
	public PipelineStatusEnum Status { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.PipelineCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty(nameof(PipelineLog.Pipeline))]
	public virtual ICollection<PipelineLog> PipelineLogs { get; } = new List<PipelineLog>();

	[InverseProperty(nameof(PipelineInstruction.Pipeline))]
	public virtual ICollection<PipelineInstruction> PipelineInstructions { get; } = new List<PipelineInstruction>();

	[InverseProperty(nameof(Postgres.PipelineTrigger.Pipeline))]
	public virtual PipelineTrigger PipelineTrigger { get; set; } = null!;

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.PipelineUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
