namespace Houston.Infrastructure.Configurations {
	public class ConnectorFunctionInputConfiguration : IEntityTypeConfiguration<ConnectorFunctionInput> {
		public void Configure(EntityTypeBuilder<ConnectorFunctionInput> builder) {
			builder.HasKey(e => e.Id).HasName("ConnectorFunctionInput_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.ConnectorFunctionHistory).WithMany(p => p.ConnectorFunctionInputs)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunctionInput_ConnectorFunctionHistory_id_fk");

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorFunctionInputCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorFunctionInputUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
