namespace Houston.Infrastructure.Configurations {
	public class ConnectorConfiguration : IEntityTypeConfiguration<Connector> {
		public void Configure(EntityTypeBuilder<Connector> entity) {
			entity.HasKey(e => e.Id).HasName("Connector_pk");

			entity.ToTable("Connector", "houston_v2");

			entity.HasIndex(e => e.Name, "Connector_ukey").IsUnique();

			entity.HasIndex(e => e.CreatedBy, "IX_Connector_created_by");

			entity.HasIndex(e => e.UpdatedBy, "IX_Connector_updated_by");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");

			entity.Property(e => e.Active).HasColumnName("active");

			entity.Property(e => e.CreatedBy).HasColumnName("created_by");

			entity.Property(e => e.CreationDate)
				.HasPrecision(3)
				.HasColumnName("creation_date");

			entity.Property(e => e.Description).HasColumnName("description");

			entity.Property(e => e.FriendlyName)
				.HasColumnType("character varying")
				.HasColumnName("friendly_name");

			entity.Property(e => e.LastUpdate)
				.HasPrecision(3)
				.HasColumnName("last_update");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");

			entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorCreatedByNavigations)
				.HasForeignKey(d => d.CreatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_created_by");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorUpdatedByNavigations)
				.HasForeignKey(d => d.UpdatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_updated_by");
		}
	}
}
