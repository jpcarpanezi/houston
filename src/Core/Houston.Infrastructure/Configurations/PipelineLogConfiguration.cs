namespace Houston.Infrastructure.Configurations {
	public class PipelineLogConfiguration : IEntityTypeConfiguration<PipelineLog> {
		public void Configure(EntityTypeBuilder<PipelineLog> builder) {
			builder.HasKey(e => e.Id).HasName("PipelineLog_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.Pipeline).WithMany(p => p.PipelineLogs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			builder.HasOne(d => d.TriggeredByNavigation).WithMany(p => p.PipelineLogTriggeredByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_triggered_by_fk");

			builder.HasOne(d => d.PipelineInstruction).WithMany(p => p.PipelineLogs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("PipelineInstruction_id_instruction_with_error");
		}
	}
}
