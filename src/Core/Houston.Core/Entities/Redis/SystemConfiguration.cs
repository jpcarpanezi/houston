namespace Houston.Core.Entities.Redis {
	public class SystemConfiguration {
		public string RegistryAddress { get; set; } = null!;

		public string RegistryEmail { get; set; } = null!;

		public string RegistryUsername { get; set; } = null!;

		public string RegistryPassword { get; set; } = null!;

		public string ContainerImage { get; set; } = null!;

		public string ImageTag { get; set; } = null!;

		public bool IsFirstAccess { get; set; }

		public SystemConfiguration() { }
	}
}
