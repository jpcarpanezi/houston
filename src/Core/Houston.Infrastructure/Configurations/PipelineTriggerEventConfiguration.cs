namespace Houston.Infrastructure.Configurations {
	public class PipelineTriggerEventConfiguration : IEntityTypeConfiguration<PipelineTriggerEvent> {
		public void Configure(EntityTypeBuilder<PipelineTriggerEvent> builder) {
			builder.HasKey(e => e.Id).HasName("PipelineTriggerEvent_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.PipelineTrigger).WithMany(p => p.PipelineTriggerEvents)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerEvent_pipeline_trigger_id_fk");

			builder.HasOne(d => d.TriggerEvent).WithMany(p => p.PipelineTriggerEvents)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineTriggerEvent_trigger_event_id_fk");
		}
	}
}
