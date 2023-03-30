using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("TriggerEvent", Schema = "houston")]
public partial class TriggerEvent {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("value", TypeName = "character varying")]
	public string Value { get; set; } = null!;

	[InverseProperty(nameof(PipelineTrigger.TriggerEvent))]
	public virtual ICollection<PipelineTrigger> PipelineTriggers { get; } = new List<PipelineTrigger>();
}
