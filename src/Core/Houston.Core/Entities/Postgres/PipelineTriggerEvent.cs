namespace Houston.Core.Entities.Postgres;

[Table("PipelineTriggerEvent", Schema = "houston")]
public partial class PipelineTriggerEvent {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_trigger_id")]
	public Guid PipelineTriggerId { get; set; }

	[Column("trigger_event_id")]
	public Guid TriggerEventId { get; set; }

	[ForeignKey(nameof(PipelineTriggerId))]
	[InverseProperty(nameof(Postgres.PipelineTrigger.PipelineTriggerEvents))]
	public virtual PipelineTrigger PipelineTrigger { get; set; } = null!;

	[InverseProperty(nameof(PipelineTriggerFilter.PipelineTriggerEvent))]
	public virtual ICollection<PipelineTriggerFilter> PipelineTriggerFilters { get; set; } = new List<PipelineTriggerFilter>();

	[ForeignKey(nameof(TriggerEventId))]
	[InverseProperty(nameof(Postgres.TriggerEvent.PipelineTriggerEvents))]
	public virtual TriggerEvent TriggerEvent { get; set; } = null!;
}
