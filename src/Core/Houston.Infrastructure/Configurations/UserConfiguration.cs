namespace Houston.Infrastructure.Configurations {
	public class UserConfiguration : IEntityTypeConfiguration<User> {
		public void Configure(EntityTypeBuilder<User> builder) {
			builder.HasKey(e => e.Id).HasName("User_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasOne(d => d.CreatedByNavigation).WithMany(p => p.InverseCreatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_created_by");

			builder.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.InverseUpdatedByNavigation)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_User_id_updated_by");
		}
	}
}
