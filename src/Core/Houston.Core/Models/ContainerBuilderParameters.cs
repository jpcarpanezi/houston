namespace Houston.Core.Models {
	public class ContainerBuilderParameters {
		public string RegistryUsername { get; set; } = null!;

		public string RegistryEmail { get; set; } = null!;

		public string RegistryPassword { get; set; } = null!;

		public string? RegistryAddress { get; set; }

		public string FromImage { get; set; } = null!;

		public string Tag { get; set; } = null!;

		public string ContainerName { get; set; } = null!;

		public string ContainerId { get; set; } = null!;

		public List<string> Binds { get; set; } = new List<string>();

		public string DeployKey { get; set; } = null!;

		public string SourceGit { get; set; } = null!;

		public List<PipelineInstruction> PipelineInstructions { get; set; } = new List<PipelineInstruction>();

		public ContainerBuilderParameters() { }

		public ContainerBuilderParameters(string registryUsername, string registryEmail, string registryPassword, string? registryAddress, string fromImage, string tag, string containerName, string containerId, List<string> binds, string deployKey, string sourceGit, List<PipelineInstruction> pipelineInstructions) {
			RegistryUsername = registryUsername ?? throw new ArgumentNullException(nameof(registryUsername));
			RegistryEmail = registryEmail ?? throw new ArgumentNullException(nameof(registryEmail));
			RegistryPassword = registryPassword ?? throw new ArgumentNullException(nameof(registryPassword));
			RegistryAddress = registryAddress;
			FromImage = fromImage ?? throw new ArgumentNullException(nameof(fromImage));
			Tag = tag ?? throw new ArgumentNullException(nameof(tag));
			ContainerName = containerName ?? throw new ArgumentNullException(nameof(containerName));
			ContainerId = containerId ?? throw new ArgumentNullException(nameof(containerId));
			Binds = binds ?? throw new ArgumentNullException(nameof(binds));
			DeployKey = deployKey ?? throw new ArgumentNullException(nameof(deployKey));
			SourceGit = sourceGit ?? throw new ArgumentNullException(nameof(sourceGit));
			PipelineInstructions = pipelineInstructions ?? throw new ArgumentNullException(nameof(pipelineInstructions));
		}
	}
}