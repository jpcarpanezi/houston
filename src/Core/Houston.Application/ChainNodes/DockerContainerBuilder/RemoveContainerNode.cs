using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;
using Microsoft.Extensions.Logging;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class RemoveContainerNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<RemoveContainerNode> _logger;

		public RemoveContainerNode(IContainerBuilderChainService next, IDockerClient client, ILogger<RemoveContainerNode> logger) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			_logger.LogInformation("Removing container {ContainerId}", parameters.ContainerId);
			await _client.Containers.StopContainerAsync(parameters.ContainerId, new ContainerStopParameters());
			await _client.Containers.RemoveContainerAsync(parameters.ContainerId, new ContainerRemoveParameters());

			return solicitation;
		}
	}
}
