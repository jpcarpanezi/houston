namespace Houston.Infrastructure.Configurations {
	public class ConnectorFunctionConfiguration : IEntityTypeConfiguration<ConnectorFunction> {
		public void Configure(EntityTypeBuilder<ConnectorFunction> builder) {
			builder.HasKey(e => e.Id).HasName("ConnectorFunction_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.Connector).WithMany(p => p.ConnectorFunction)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Connector_id_connector_id_fk");

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorFunctionCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorFunctionUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
