namespace Houston.Infrastructure.Configurations {
	public class PipelineLogConfiguration : IEntityTypeConfiguration<PipelineLog> {
		public void Configure(EntityTypeBuilder<PipelineLog> entity) {
			entity.HasKey(e => e.Id).HasName("PipelineLog_pk");

			entity.ToTable("PipelineLog", "houston_v2");

			entity.HasIndex(e => e.PipelineId, "IX_PipelineLog_pipeline_id");

			entity.HasIndex(e => e.TriggeredBy, "IX_PipelineLog_triggered_by");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");

			entity.Property(e => e.Duration).HasColumnName("duration");

			entity.Property(e => e.ExitCode).HasColumnName("exit_code");

			entity.Property(e => e.PipelineId).HasColumnName("pipeline_id");

			entity.Property(e => e.SpecFile).HasColumnName("spec_file");

			entity.Property(e => e.StartTime)
				.HasPrecision(3)
				.HasColumnName("start_time");

			entity.Property(e => e.Stdout).HasColumnName("stdout");

			entity.Property(e => e.StepError).HasColumnName("step_error");

			entity.Property(e => e.TriggeredBy).HasColumnName("triggered_by");

			entity.HasOne(d => d.Pipeline).WithMany(p => p.PipelineLogs)
				.HasForeignKey(d => d.PipelineId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			entity.HasOne(d => d.TriggeredByNavigation).WithMany(p => p.PipelineLogs)
				.HasForeignKey(d => d.TriggeredBy)
				.HasConstraintName("User_id_triggered_by_fk");
		}
	}
}
