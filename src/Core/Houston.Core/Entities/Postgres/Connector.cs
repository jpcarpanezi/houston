using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Houston.Core.Entities.Postgres;

[Table("Connector", Schema = "houston")]
public partial class Connector {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("name", TypeName = "character varying")]
	public string Name { get; set; } = null!;

	[Column("description")]
	public string? Description { get; set; }

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) without time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) without time zone")]
	public DateTime LastUpdate { get; set; }

	[InverseProperty("Connector")]
	public virtual ICollection<ConnectorFunction> ConnectorFunction { get; } = new List<ConnectorFunction>();

	[ForeignKey("CreatedBy")]
	[InverseProperty("ConnectorCreatedByNavigation")]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey("UpdatedBy")]
	[InverseProperty("ConnectorUpdatedByNavigation")]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
