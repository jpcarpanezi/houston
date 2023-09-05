namespace Houston.Application.CommandHandlers.WorkerCommandHandlers.BuildConnectorFunction {
	public record WorkerBuildConnectorFunctionCommand : ContainerBaseCommand, IRequest<BuildConnectorFunctionViewModel> {
		public byte[] Script { get; init; }

		public byte[] Package { get; init; }

		public WorkerBuildConnectorFunctionCommand(byte[] script, byte[] package, string containerImage, string imageTag, string registryEmail, string registryUsername, string registryPassword, string containerName, List<string> env) : base(containerImage, imageTag, registryEmail, registryUsername, registryPassword, containerName, env, null, null) {
			Script = script ?? throw new ArgumentNullException(nameof(script));
			Package = package ?? throw new ArgumentNullException(nameof(package));
		}
	}
}
