namespace Houston.Infrastructure.Configurations {
	public class UserConfiguration : IEntityTypeConfiguration<User> {
		public void Configure(EntityTypeBuilder<User> entity) {
			entity.HasKey(e => e.Id).HasName("User_pk");

			entity.ToTable("User", "houston_v2");

			entity.HasIndex(e => e.CreatedBy, "IX_User_created_by");

			entity.HasIndex(e => e.UpdatedBy, "IX_User_updated_by");

			entity.HasIndex(e => e.Email, "User_email_uq").IsUnique();

			entity.Property(e => e.Id)
				.ValueGeneratedNever()
				.HasColumnName("id");

			entity.Property(e => e.Active).HasColumnName("active");
			entity.Property(e => e.CreatedBy).HasColumnName("created_by");
			entity.Property(e => e.CreationDate)
				.HasPrecision(3)
				.HasColumnName("creation_date");

			entity.Property(e => e.Email)
				.HasColumnType("character varying")
				.HasColumnName("email");

			entity.Property(e => e.FirstAccess).HasColumnName("first_access");
			entity.Property(e => e.LastUpdate)
				.HasPrecision(3)
				.HasColumnName("last_update");

			entity.Property(e => e.Name)
				.HasColumnType("character varying")
				.HasColumnName("name");

			entity.Property(e => e.Password)
				.HasMaxLength(256)
				.HasColumnName("password");

			entity.Property(e => e.Role).HasColumnName("role");

			entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

			entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
				.HasForeignKey(d => d.CreatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_created_by");

			entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
				.HasForeignKey(d => d.UpdatedBy)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_updated_by");
		}
	}
}
