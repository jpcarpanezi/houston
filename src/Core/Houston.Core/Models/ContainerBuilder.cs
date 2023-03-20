using Houston.Core.Entities.MongoDB;

namespace Houston.Core.Models {
	public class ContainerBuilder {
		public string RegistryUsername { get; set; } = null!;

		public string RegistryEmail { get; set; } = null!;

		public string RegistryPassword { get; set; } = null!;

		public string? RegistryAddress { get; set; }

		public string FromImage { get; set; } = null!;

		public string Tag { get; set; } = null!;

		public string ContainerName { get; set; } = null!;

		public List<string> Binds { get; set; } = null!;

		public Pipeline Pipeline { get; set; } = null!;

		public ContainerBuilder() { }

		public ContainerBuilder(string registryUsername, string registryEmail, string registryPassword, string? registryAddress, string fromImage, string tag, string containerName, List<string> binds, Pipeline pipeline) {
			RegistryUsername = registryUsername ?? throw new ArgumentNullException(nameof(registryUsername));
			RegistryEmail = registryEmail ?? throw new ArgumentNullException(nameof(registryEmail));
			RegistryPassword = registryPassword ?? throw new ArgumentNullException(nameof(registryPassword));
			RegistryAddress = registryAddress;
			FromImage = fromImage ?? throw new ArgumentNullException(nameof(fromImage));
			Tag = tag ?? throw new ArgumentNullException(nameof(tag));
			ContainerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
			Binds = binds ?? throw new ArgumentNullException(nameof(binds));
			Pipeline = pipeline;
		}
	}
}