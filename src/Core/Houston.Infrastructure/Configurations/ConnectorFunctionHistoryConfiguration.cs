namespace Houston.Infrastructure.Configurations {
	public class ConnectorFunctionHistoryConfiguration : IEntityTypeConfiguration<ConnectorFunctionHistory> {
		public void Configure(EntityTypeBuilder<ConnectorFunctionHistory> builder) {
			builder.HasKey(e => e.Id).HasName("ConnectorFunctionHistory_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.ConnectorFunction).WithMany(p => p.ConnectorFunctionHistories)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunctionHistory_ConnectorFunction_id_fk");

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorFunctionHistoryCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunctionHistory_User_id_fk");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorFunctionHistoryUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("ConnectorFunctionHistory_User_id_fk2");
		}
	}
}
