namespace Houston.Infrastructure.Configurations {
	public class PipelineConfiguration : IEntityTypeConfiguration<Pipeline> {
		public void Configure(EntityTypeBuilder<Pipeline> entity) {
			entity.HasKey(e => e.Id).HasName("Pipeline_pk");

			entity.ToTable("Pipeline", "houston_v2");

			entity.HasIndex(e => e.CreatedBy, "IX_Pipeline_created_by");

			entity.HasIndex(e => e.UpdatedBy, "IX_Pipeline_updated_by");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");

			entity.Property(e => e.Active).HasColumnName("active");

			entity.Property(e => e.CreatedBy).HasColumnName("created_by");

			entity.Property(e => e.CreationDate)
				.HasPrecision(3)
				.HasColumnName("creation_date");

			entity.Property(e => e.Description)
				.HasColumnType("character varying")
				.HasColumnName("description");

			entity.Property(e => e.LastUpdate)
				.HasPrecision(3)
				.HasColumnName("last_update");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");

			entity.Property(e => e.SpecFile).HasColumnName("spec_file");

			entity.Property(e => e.Status).HasColumnName("status");

			entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineCreatedByNavigations)
				.HasForeignKey(d => d.CreatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineUpdatedByNavigations)
				.HasForeignKey(d => d.UpdatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
