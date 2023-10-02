namespace Houston.Infrastructure.Configurations {
	public class PipelineTriggerFilterConfiguration : IEntityTypeConfiguration<PipelineTriggerFilter> {
		public void Configure(EntityTypeBuilder<PipelineTriggerFilter> builder) {
			builder.HasKey(e => e.Id).HasName("PipelineTriggerFilter_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.PipelineTriggerEvent).WithMany(p => p.PipelineTriggerFilters)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerFilter_pipeline_trigger_event_id_fk");

			builder.HasOne(d => d.TriggerFilter).WithMany(p => p.PipelineTriggerFilters)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerFilter_trigger_filter_id_fk");
		}
	}
}
