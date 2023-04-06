using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("PipelineTriggerEvent", Schema = "houston")]
public partial class PipelineTriggerEvent {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_trigger_id")]
	public Guid PipelineTriggerId { get; set; }

	[Column("pipeline_event_id")]
	public Guid PipelineEventId { get; set; }

	[ForeignKey(nameof(PipelineTriggerId))]
	[InverseProperty(nameof(Postgres.PipelineTrigger.PipelineTriggerEvents))]
	public virtual PipelineTrigger PipelineTrigger { get; set; } = null!;

	[InverseProperty(nameof(PipelineTriggerFilter.PipelineTriggerEvent))]
	public virtual ICollection<PipelineTriggerFilter> PipelineTriggerFilters { get; set; } = new List<PipelineTriggerFilter>();

	[ForeignKey(nameof(PipelineEventId))]
	[InverseProperty(nameof(Postgres.TriggerEvent.PipelineTriggerEvents))]
	public virtual TriggerEvent TriggerEvent { get; set; } = null!;
}
