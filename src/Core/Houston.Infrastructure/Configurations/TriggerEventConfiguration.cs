namespace Houston.Infrastructure.Configurations {
	public class TriggerEventConfiguration : IEntityTypeConfiguration<TriggerEvent> {
		public void Configure(EntityTypeBuilder<TriggerEvent> builder) {
			builder.HasKey(e => e.Id).HasName("TriggerEvent_pk");

			builder.Property(e => e.Id).ValueGeneratedNever();

			builder.HasData(
				new TriggerEvent { Id = Guid.Parse("c0437ca0-a971-4d40-99f6-2a3c35e6fb41"), Value = "push" },
				new TriggerEvent { Id = Guid.Parse("e9b3eb7e-526b-4f89-968c-7cc0f60228cd"), Value = "pull_request" }
			);
		}
	}
}
