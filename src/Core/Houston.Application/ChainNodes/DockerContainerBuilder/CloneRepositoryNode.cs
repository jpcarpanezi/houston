using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Exceptions;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CloneRepositoryNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public CloneRepositoryNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
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

			if (inspectContainer.ExitCode != 0)
				throw new ContainerBuilderException("An error occurred when cloning git repository.", $"{stdout}\n{stderr}\n");

			await stream.ReadOutputToEndAsync(default);

			return await Next.Handler(solicitation, parameters);
		}
	}
}
