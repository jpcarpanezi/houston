namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CloneRepositoryNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<CloneRepositoryNode> _logger;

		public CloneRepositoryNode(IContainerBuilderChainService next, IDockerClient client, ILogger<CloneRepositoryNode> logger) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			_logger.LogInformation("Cloning git repository {sourceGit}.", parameters.SourceGit);

			var generateCloneScriptResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
				Cmd = new List<string> {
					"/bin/bash",
					"-c",
					$"apt-get update -y; apt-get install -y git-all wget; chmod 400 /root/.ssh/id_rsa; ssh-keyscan -H github.com >> /root/.ssh/known_hosts; git clone {parameters.SourceGit} /src; cd /src"
				},
				Detach = false,
				Tty = false,
				AttachStdout = true,
				AttachStderr = true,
				AttachStdin = true
			});

			var stream = await _client.Exec.StartAndAttachContainerExecAsync(generateCloneScriptResponse.ID, false, default);
			var inspectContainer = await _client.Exec.InspectContainerExecAsync(generateCloneScriptResponse.ID);
			var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

			if (inspectContainer.ExitCode != 0) {
				_logger.LogError("An error occurred when cloning git repository {sourceGit}. {Stdout} {Stderr}", parameters.SourceGit, stdout, stderr);
				throw new ContainerBuilderException("An error occurred when cloning git repository.", $"{stdout}\n{stderr}\n");
			}
			_logger.LogInformation("Git repository {sourceGit} has been cloned successfully.", parameters.SourceGit);

			return await Next.Handler(solicitation, parameters);
		}
	}
}
