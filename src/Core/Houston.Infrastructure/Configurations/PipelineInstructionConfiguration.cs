namespace Houston.Infrastructure.Configurations {
	public class PipelineInstructionConfiguration : IEntityTypeConfiguration<PipelineInstruction> {
		public void Configure(EntityTypeBuilder<PipelineInstruction> builder) {
			builder.HasKey(e => e.Id).HasName("PipelineInstruction_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineInstructionCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_id");

			builder.HasOne(d => d.Pipeline).WithMany(p => p.PipelineInstructions)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Pipeline_id_pipeline_id_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineInstructionUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");

			builder.HasOne(d => d.ConnectorFunctionHistory).WithMany(p => p.PipelineInstructions)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("PipelineInstruction_ConnectorFunctionHistory_id_fk");
		}
	}
}
