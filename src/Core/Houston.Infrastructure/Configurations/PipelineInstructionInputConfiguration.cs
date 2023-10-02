namespace Houston.Infrastructure.Configurations {
	public class PipelineInstructionInputConfiguration : IEntityTypeConfiguration<PipelineInstructionInput> {
		public void Configure(EntityTypeBuilder<PipelineInstructionInput> builder) {
			builder.HasKey(e => e.Id).HasName("PipelineInstructionInput_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.PipelineInstructionInputCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			builder.HasOne(d => d.ConnectorFunctionInput).WithMany(p => p.PipelineInstructionInputs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunctionInput_id_input_id_fk");

			builder.HasOne(d => d.PipelineInstruction).WithMany(p => p.PipelineInstructionInputs)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("PipelineInstruction_id_input_id_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.PipelineInstructionInputUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
