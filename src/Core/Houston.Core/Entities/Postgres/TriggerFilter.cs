﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("TriggerFilter", Schema = "houston")]
public partial class TriggerFilter {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("value", TypeName = "character varying")]
	public string Value { get; set; } = null!;

	[InverseProperty(nameof(PipelineTrigger.TriggerFilter))]
	public virtual ICollection<PipelineTrigger> PipelineTriggers { get; } = new List<PipelineTrigger>();
}