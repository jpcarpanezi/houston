using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("PipelineTrigger", Schema = "houston")]
public partial class PipelineTrigger {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_id")]
	public Guid PipelineId { get; set; }

	[Column("event_id")]
	public Guid EventId { get; set; }

	[Column("filter_id")]
	public Guid FilterId { get; set; }

	[Column("filter_values", TypeName = "character varying[]")]
	public string[]? FilterValues { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) without time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) without time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.PipelineTriggerCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey(nameof(EventId))]
	[InverseProperty(nameof(Postgres.TriggerEvent.PipelineTriggers))]
	public virtual TriggerEvent TriggerEvent { get; set; } = null!;

	[ForeignKey(nameof(FilterId))]
	[InverseProperty(nameof(Postgres.TriggerFilter.PipelineTriggers))]
	public virtual TriggerFilter TriggerFilter { get; set; } = null!;

	[ForeignKey(nameof(PipelineId))]
	[InverseProperty(nameof(Postgres.Pipeline.PipelineTriggers))]
	public virtual Pipeline Pipeline { get; set; } = null!;

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.PipelineTriggerUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
