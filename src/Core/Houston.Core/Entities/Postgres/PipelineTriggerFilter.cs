namespace Houston.Core.Entities.Postgres;

[Table("PipelineTriggerFilter", Schema = "houston")]
public partial class PipelineTriggerFilter {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_trigger_event_id")]
	public Guid PipelineTriggerEventId { get; set; }

	[Column("trigger_filter_id")]
	public Guid TriggerFilterId { get; set; }

	[Column("filter_values", TypeName = "character varying[]")]
	public string[] FilterValues { get; set; } = null!;

	[ForeignKey(nameof(PipelineTriggerEventId))]
	[InverseProperty(nameof(Postgres.PipelineTriggerEvent.PipelineTriggerFilters))]
	public virtual PipelineTriggerEvent PipelineTriggerEvent { get; set; } = null!;

	[ForeignKey(nameof(TriggerFilterId))]
	[InverseProperty(nameof(Postgres.TriggerFilter.PipelineTriggerFilters))]
	public virtual TriggerFilter TriggerFilter { get; set; } = null!;
}

