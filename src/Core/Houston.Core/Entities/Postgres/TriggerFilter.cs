﻿namespace Houston.Core.Entities.Postgres;

[Table("TriggerFilter", Schema = "houston")]
public partial class TriggerFilter {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("value", TypeName = "character varying")]
	public string Value { get; set; } = null!;

	[InverseProperty(nameof(PipelineTriggerFilter.TriggerFilter))]
	public virtual ICollection<PipelineTriggerFilter> PipelineTriggerFilters { get; set; } = new List<PipelineTriggerFilter>();
}
