namespace Houston.Core.Models.Pipelines {
	public record RunPipelineParameters {
		public string RegistryUsername { get; init; } = null!;

		public string RegistryEmail { get; init; } = null!;

		public string RegistryPassword { get; init; } = null!;

		public string FromImage { get; init; } = null!;

		public string Tag { get; init; } = null!;

		public string ContainerName { get; init; } = null!;

		public string DeployKey { get; init; } = null!;

		public string SourceGit { get; init; } = null!;
	}
}
