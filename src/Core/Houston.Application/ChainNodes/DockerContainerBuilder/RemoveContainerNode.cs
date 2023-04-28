using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class RemoveContainerNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public RemoveContainerNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			await _client.Containers.StopContainerAsync(parameters.ContainerId, new ContainerStopParameters());
			await _client.Containers.RemoveContainerAsync(parameters.ContainerId, new ContainerRemoveParameters());

			return solicitation;
		}
	}
}
