﻿namespace Houston.Core.Entities.Postgres;

[Table("PipelineTrigger", Schema = "houston")]
public partial class PipelineTrigger {
	[Key]
	[Column("id")]
	public Guid Id { get; set; }

	[Column("pipeline_id")]
	public Guid PipelineId { get; set; }

	[Column("source_git", TypeName = "character varying")]
	public string SourceGit { get; set; } = null!;

	[Column("private_key", TypeName = "character varying")]
	public string PrivateKey { get; set; } = null!;

	[Column("public_key", TypeName = "character varying")]
	public string PublicKey { get; set; } = null!;

	[Column("key_revealed")]
	public bool KeyRevealed { get; set; }

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
	[InverseProperty(nameof(Postgres.Pipeline.PipelineTrigger))]
	public virtual Pipeline Pipeline { get; set; } = null!;

	[InverseProperty(nameof(PipelineTriggerEvent.PipelineTrigger))]
	public virtual ICollection<PipelineTriggerEvent> PipelineTriggerEvents { get; set; } = new List<PipelineTriggerEvent>();

	[ForeignKey(nameof(UpdatedBy))]
	[InverseProperty(nameof(User.PipelineTriggerUpdatedByNavigation))]
	public virtual User UpdatedByNavigation { get; set; } = null!;
}
