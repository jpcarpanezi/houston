namespace Houston.Core.Commands {
	public abstract record ContainerBaseCommand {
		public string ContainerImage { get; init; } = null!;

		public string ImageTag { get; init; } = null!;

		public string RegistryEmail { get; init; } = null!;

		public string RegistryUsername { get; init; } = null!;

		public string RegistryPassword { get; init; } = null!;

		public string ContainerName { get; init; } = null!;

		public List<string> Env { get; init; } = new List<string>();

		public string? ContainerId { get; set; } = null!;

		protected ContainerBaseCommand(string containerImage, string imageTag, string registryEmail, string registryUsername, string registryPassword, string containerName, List<string> env, string? containerId) {
			ContainerImage = containerImage ?? throw new ArgumentNullException(nameof(containerImage));
			ImageTag = imageTag ?? throw new ArgumentNullException(nameof(imageTag));
			RegistryEmail = registryEmail ?? throw new ArgumentNullException(nameof(registryEmail));
			RegistryUsername = registryUsername ?? throw new ArgumentNullException(nameof(registryUsername));
			RegistryPassword = registryPassword ?? throw new ArgumentNullException(nameof(registryPassword));
			ContainerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
			Env = env ?? throw new ArgumentNullException(nameof(env));
			ContainerId = containerId;
		}
	}
}
