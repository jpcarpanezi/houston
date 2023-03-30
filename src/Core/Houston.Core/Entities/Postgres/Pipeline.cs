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

	[Column("status")]
	public PipelineStatusEnum Status { get; set; }

	[Column("source_git", TypeName = "character varying")]
	public string SourceGit { get; set; } = null!;

	[Column("deploy_key", TypeName = "character varying")]
	public string DeployKey { get; set; } = null!;

	[Column("secret")]
	[StringLength(256)]
	public string Secret { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) without time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) without time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey("CreatedBy")]
	[InverseProperty("PipelineCreatedByNavigation")]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[InverseProperty("Pipeline")]
	public virtual ICollection<PipelineLog> PipelineLog { get; } = new List<PipelineLog>();

	[InverseProperty("Pipeline")]
	public virtual ICollection<PipelineInstruction> PipelineInstruction { get; } = new List<PipelineInstruction>();

	[InverseProperty("Pipeline")]
	public virtual ICollection<PipelineTrigger> PipelineTrigger { get; } = new List<PipelineTrigger>();

	[ForeignKey("UpdatedBy")]
	[InverseProperty("PipelineUpdatedByNavigation")]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
