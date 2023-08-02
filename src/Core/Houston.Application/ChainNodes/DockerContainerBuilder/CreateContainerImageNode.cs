namespace Houston.Application.ChainNodes.DockerContainerBuilder {
	public class CreateContainerImageNode : IContainerBuilderChainService {
		public IContainerBuilderChainService Next { get; set; }

		private readonly IDockerClient _client;
		private readonly ILogger<CreateContainerImageNode> _logger;

		public CreateContainerImageNode(IContainerBuilderChainService next, IDockerClient client, ILogger<CreateContainerImageNode> logger) {
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
				new Progress<JSONMessage>()
			);

			_logger.LogInformation("Container image created");

			return await Next.Handler(solicitation, parameters);
		}
	}
}
