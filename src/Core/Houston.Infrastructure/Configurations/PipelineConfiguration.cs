namespace Houston.Infrastructure.Configurations {
	public class PipelineConfiguration : IEntityTypeConfiguration<Pipeline> {
		public void Configure(EntityTypeBuilder<Pipeline> builder) {
			builder.HasKey(e => e.Id).HasName("Pipeline_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
