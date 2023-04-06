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

	[Column("source_git", TypeName = "character varying")]
	public string SourceGit { get; set; } = null!;

	[Column("deploy_key", TypeName = "character varying")]
	public string DeployKey { get; set; } = null!;

	[NotMapped]
	public string PublicKey { get; set; } = null!;

	[Column("secret")]
	[StringLength(256)]
	public string Secret { get; set; } = null!;

	[Column("created_by")]
	public Guid CreatedBy { get; set; }

	[Column("creation_date", TypeName = "timestamp(3) with time zone")]
	public DateTime CreationDate { get; set; }

	[Column("updated_by")]
	public Guid UpdatedBy { get; set; }

	[Column("last_update", TypeName = "timestamp(3) with time zone")]
	public DateTime LastUpdate { get; set; }

	[ForeignKey(nameof(CreatedBy))]
	[InverseProperty(nameof(User.PipelineTriggerCreatedByNavigation))]
	public virtual User CreatedByNavigation { get; set; } = null!;

	[ForeignKey(nameof(PipelineId))]
	[InverseProperty(nameof(Postgres.Pipeline.PipelineTriggers))]
	public virtual Pipeline Pipeline { get; set; } = null!;

	[InverseProperty(nameof(PipelineTriggerEvent.PipelineTrigger))]
	public virtual ICollection<PipelineTriggerEvent> PipelineTriggerEvents { get; set; } = new List<PipelineTriggerEvent>();

	[InverseProperty(nameof(PipelineTriggerFilter.PipelineTrigger))]
	public virtual ICollection<PipelineTriggerFilter> PipelineTriggerFilters { get; set; } = new List<PipelineTriggerFilter>();

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.PipelineTriggerUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
