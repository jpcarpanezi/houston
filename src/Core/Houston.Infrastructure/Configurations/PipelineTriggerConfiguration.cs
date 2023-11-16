namespace Houston.Infrastructure.Configurations {
	public class PipelineTriggerConfiguration : IEntityTypeConfiguration<PipelineTrigger> {
		public void Configure(EntityTypeBuilder<PipelineTrigger> entity) {
			entity.HasKey(e => e.Id).HasName("PipelineTrigger_pk");

			entity.ToTable("PipelineTrigger", "houston_v2");

			entity.HasIndex(e => e.CreatedBy, "IX_PipelineTrigger_created_by");

			entity.HasIndex(e => e.PipelineId, "IX_PipelineTrigger_pipeline_id").IsUnique();

			entity.HasIndex(e => e.UpdatedBy, "IX_PipelineTrigger_updated_by");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");

			entity.Property(e => e.CreatedBy).HasColumnName("created_by");
			entity.Property(e => e.CreationDate)
				.HasPrecision(3)
				.HasColumnName("creation_date");

			entity.Property(e => e.KeyRevealed).HasColumnName("key_revealed");
			entity.Property(e => e.LastUpdate)
				.HasPrecision(3)
				.HasColumnName("last_update");

			entity.Property(e => e.PipelineId).HasColumnName("pipeline_id");
			entity.Property(e => e.PrivateKey)
				.HasColumnType("character varying")
				.HasColumnName("private_key");

			entity.Property(e => e.PublicKey)
				.HasColumnType("character varying")
				.HasColumnName("public_key");

			entity.Property(e => e.Secret)
				.HasMaxLength(256)
				.HasColumnName("secret");
			entity.Property(e => e.SourceGit)
				.HasColumnType("character varying")
				.HasColumnName("source_git");

			entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineTriggerCreatedByNavigations)
				.HasForeignKey(d => d.CreatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.Pipeline).WithOne(p => p.PipelineTrigger)
				.HasForeignKey<PipelineTrigger>(d => d.PipelineId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineTriggerUpdatedByNavigations)
				.HasForeignKey(d => d.UpdatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
