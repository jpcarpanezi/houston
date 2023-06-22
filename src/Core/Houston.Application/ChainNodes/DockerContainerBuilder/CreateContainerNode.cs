using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreateContainerNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<CreateContainerNode> _logger;

		public CreateContainerNode(IContainerBuilderChainService next, IDockerClient client, ILogger<CreateContainerNode> logger) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			_logger.LogInformation("Creating container {containerName} from image {fromImage}:{tag}...", parameters.ContainerName, parameters.FromImage, parameters.Tag);

			var container = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
				Image = $"{parameters.FromImage}:{parameters.Tag}",
				Name = parameters.ContainerName,
				Tty = true,
				AttachStdout = true,
				AttachStderr = true,
				AttachStdin = true,
				HostConfig = new HostConfig {
					Privileged = true,
					Binds = parameters.Binds
				}
			});

			parameters.ContainerId = container.ID[..12];
			await _client.Containers.StartContainerAsync(parameters.ContainerId, new ContainerStartParameters());

			_logger.LogInformation("Container {containerName} created with id {containerId}.", parameters.ContainerName, parameters.ContainerId);

			return await Next.Handler(solicitation, parameters);
		}
	}
}
