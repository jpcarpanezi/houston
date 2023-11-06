namespace Houston.Infrastructure.Configurations {
	public class ConnectorConfiguration : IEntityTypeConfiguration<Connector> {
		public void Configure(EntityTypeBuilder<Connector> builder) {
			builder.HasKey(e => e.Id).HasName("Connector_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_created_by");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_updated_by");
		}
	}
}
