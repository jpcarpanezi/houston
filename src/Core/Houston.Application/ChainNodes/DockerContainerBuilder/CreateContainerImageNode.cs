using Docker.DotNet;
using Docker.DotNet.Models;
using Houston.Core.Interfaces.Services;
using Houston.Core.Models;

namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreateContainerImageNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;

		public CreateContainerImageNode(IContainerBuilderChainService next, IDockerClient client) {
			Next = next;
			_client = client ?? throw new ArgumentNullException(nameof(client));
		}

		public async Task<ContainerChainResponse> Handler(ContainerChainResponse solicitation, ContainerBuilderParameters parameters) {
			await _client.Images.CreateImageAsync(
				new ImagesCreateParameters() {
					FromImage = parameters.FromImage,
					Tag = parameters.Tag
				},
				new AuthConfig() {
					Email = parameters.RegistryEmail,
					Username = parameters.RegistryUsername,
					Password = parameters.RegistryPassword
				},
				new Progress<JSONMessage>()
			);

			return await Next.Handler(solicitation, parameters);
		}
	}
}
