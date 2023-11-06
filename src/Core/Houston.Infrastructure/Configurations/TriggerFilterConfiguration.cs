namespace Houston.Infrastructure.Configurations {
	public class TriggerFilterConfiguration : IEntityTypeConfiguration<TriggerFilter> {
		public void Configure(EntityTypeBuilder<TriggerFilter> builder) {
			builder.HasKey(e => e.Id).HasName("TriggerFilter_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasData(
				new TriggerFilter { Id = Guid.Parse("24a42711-ed13-405b-8527-b5e53c680b4d"), Value = "branches" },
				new TriggerFilter { Id = Guid.Parse("f7c800a4-1f05-478f-9a0b-46fed919eae2"), Value = "paths" },
				new TriggerFilter { Id = Guid.Parse("aecde3fd-e2cf-4817-9701-178305697f46"), Value = "tags" },
				new TriggerFilter { Id = Guid.Parse("e859f16a-588b-46e2-b9f4-f7b60051e387"), Value = "types" }
			);
		}
	}
}
