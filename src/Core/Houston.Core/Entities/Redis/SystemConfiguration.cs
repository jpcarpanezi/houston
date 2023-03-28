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

		public SystemConfiguration(string registryAddress, string registryEmail, string registryUsername, string registryPassword, string containerImage, string imageTag, bool isFirstAccess) {
			RegistryAddress = registryAddress ?? throw new ArgumentNullException(nameof(registryAddress));
			RegistryEmail = registryEmail ?? throw new ArgumentNullException(nameof(registryEmail));
			RegistryUsername = registryUsername ?? throw new ArgumentNullException(nameof(registryUsername));
			RegistryPassword = registryPassword ?? throw new ArgumentNullException(nameof(registryPassword));
			ContainerImage = containerImage ?? throw new ArgumentNullException(nameof(containerImage));
			ImageTag = imageTag ?? throw new ArgumentNullException(nameof(imageTag));
			IsFirstAccess = isFirstAccess;
		}
	}
}
