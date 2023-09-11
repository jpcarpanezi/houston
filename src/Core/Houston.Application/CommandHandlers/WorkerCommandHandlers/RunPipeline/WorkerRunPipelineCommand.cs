namespace Houston.Application.CommandHandlers.WorkerCommandHandlers.RunPipeline {
	public record WorkerRunPipelineCommand : ContainerBaseCommand, IRequest<RunPipelineViewModel> {
		public Pipeline Pipeline { get; init; }

		public string Branch { get; init; }

		public WorkerRunPipelineCommand(Pipeline pipeline, string branch, string containerImage, string imageTag, string registryEmail, string registryUsername, string registryPassword, string containerName, List<string> env) : base(containerImage, imageTag, registryEmail, registryUsername, registryPassword, containerName, env, null, null) {
			Pipeline = pipeline ?? throw new ArgumentNullException(nameof(pipeline));
			Branch = branch ?? throw new ArgumentNullException(nameof(branch));
		}
	}
}
