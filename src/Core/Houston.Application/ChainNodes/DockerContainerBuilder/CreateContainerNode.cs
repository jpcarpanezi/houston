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
			_logger.LogInformation("Creating container image...");

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
				new Progress<JSONMessage>() { ProgressHandler = new ProgressHandle(_logger.LogDebug) } // added to log debug information for progress
			);

			_logger.LogInformation("Container image created");

			return await Next.Handler(solicitation, parameters);
		}
	}
}
