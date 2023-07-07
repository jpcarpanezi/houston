using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Exceptions;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreatePipelineDeployKeyNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<CreatePipelineDeployKeyNode> _logger;

		public CreatePipelineDeployKeyNode(IContainerBuilderChainService next, IDockerClient client, ILogger<CreatePipelineDeployKeyNode> logger) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			_logger.LogInformation("Creating deploy key for pipeline {pipelineId}.", parameters.SourceGit);

			var deployKey = Encoding.UTF8.GetString(Convert.FromBase64String(parameters.DeployKey)).Replace("\r\n", "\n").Replace("\r", "\n");

			var generateSSHKeyCreateResponse = await _client.Exec.ExecCreateContainerAsync(parameters.ContainerId, new ContainerExecCreateParameters {
				Cmd = new List<string>
				{
					"/bin/bash",
					"-c",
					$"mkdir /root/.ssh; echo '{deployKey}' >> /root/.ssh/id_rsa"
				},
				Detach = false,
				Tty = false,
				AttachStdout = true,
				AttachStderr = true,
				AttachStdin = true
			}, default);

			var stream = await _client.Exec.StartAndAttachContainerExecAsync(generateSSHKeyCreateResponse.ID, false, default);
			var inspectContainer = await _client.Exec.InspectContainerExecAsync(generateSSHKeyCreateResponse.ID);
			var (stdout, stderr) = await stream.ReadOutputToEndAsync(default);

			if (inspectContainer.ExitCode != 0) {
				_logger.LogError("An error occurred when creating the deploy key. {Stdout} {Stderr}", stdout, stderr);
				throw new ContainerBuilderException("An error occurred when creating the deploy key.", $"{stdout}\n{stderr}\n");
			}
			_logger.LogInformation("Deploy key created for pipeline {pipelineId}.", parameters.SourceGit);

			return await Next.Handler(solicitation, parameters);
		}
	}
}
