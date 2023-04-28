using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Exceptions;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using System.Text;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreatePipelineDeployKeyNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public CreatePipelineDeployKeyNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			var deployKey = Encoding.UTF8.GetString(Convert.FromBase64String(parameters.DeployKey)).Replace("\r\n", "\n").Replace("\r", "\n");

			var generateSSHKeyCreateResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
				Cmd = new List<string> {
					"/bin/bash",
					"-c",
					$"mkdir /root/.ssh; echo '{deployKey}' >> /root/.ssh/id_rsa"
				},
				AttachStdin = true,
				Tty = true
			}, default);

			var stream = await _client.Exec.StartAndAttachContainerExecAsync(generateSSHKeyCreateResponse.ID, false, default);
			var inspectContainer = await _client.Exec.InspectContainerExecAsync(generateSSHKeyCreateResponse.ID);
			var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

			if (inspectContainer.ExitCode != 0)
				throw new ContainerBuilderException("An error occurred when creating the deploy key.", $"{stdout}\n{stderr}\n");

			return await Next.Handler(solicitation, parameters);
		}
	}
}
