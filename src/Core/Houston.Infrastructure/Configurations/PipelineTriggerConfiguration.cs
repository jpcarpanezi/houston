namespace Houston.Infrastructure.Configurations {
	public class PipelineTriggerConfiguration : IEntityTypeConfiguration<PipelineTrigger> {
		public void Configure(EntityTypeBuilder<PipelineTrigger> builder) {
			builder.HasKey(e => e.Id).HasName("PipelineTrigger_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineTriggerCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			builder.HasOne(d => d.Pipeline).WithOne(p => p.PipelineTrigger)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineTriggerUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
