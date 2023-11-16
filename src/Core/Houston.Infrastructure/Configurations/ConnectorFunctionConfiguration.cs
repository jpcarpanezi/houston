namespace Houston.Infrastructure.Configurations {
	public class ConnectorFunctionConfiguration : IEntityTypeConfiguration<ConnectorFunction> {
		public void Configure(EntityTypeBuilder<ConnectorFunction> entity) {
			entity.HasKey(e => e.Id).HasName("ConnectorFunction_pk");

			entity.ToTable("ConnectorFunction", "houston_v2");

			entity.HasIndex(e => new { e.Name, e.ConnectorId, e.Version }, "ConnectorFunction_ukey").IsUnique();

			entity.HasIndex(e => e.ConnectorId, "IX_ConnectorFunction_connector_id");

			entity.HasIndex(e => e.CreatedBy, "IX_ConnectorFunction_created_by");

			entity.HasIndex(e => e.UpdatedBy, "IX_ConnectorFunction_updated_by");

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");

			entity.Property(e => e.Active).HasColumnName("active");

			entity.Property(e => e.BuildStatus).HasColumnName("build_status");

			entity.Property(e => e.BuildStderr)
				.HasColumnType("character varying")
				.HasColumnName("build_stderr");

			entity.Property(e => e.ConnectorId).HasColumnName("connector_id");

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

			entity.Property(e => e.Package).HasColumnName("package");

			entity.Property(e => e.PackageType).HasColumnName("package_type");

			entity.Property(e => e.Script).HasColumnName("script");

			entity.Property(e => e.ScriptDist).HasColumnName("script_dist");

			entity.Property(e => e.SpecsFile).HasColumnName("specs_file");

			entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

			entity.Property(e => e.Version)
				.HasColumnType("character varying")
				.HasColumnName("version");

			entity.HasOne(d => d.Connector).WithMany(p => p.ConnectorFunctions)
				.HasForeignKey(d => d.ConnectorId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("Connector_id_connector_id_fk");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.ConnectorFunctionCreatedByNavigations)
				.HasForeignKey(d => d.CreatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_created_by_fk");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.ConnectorFunctionUpdatedByNavigations)
				.HasForeignKey(d => d.UpdatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("User_id_updated_by_fk");
		}
	}
}
