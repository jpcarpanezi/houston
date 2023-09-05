namespace Houston.Application.PipelineBehaviors {
	public class CreateContainerImageBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ContainerBaseCommand {
		private readonly IDockerClient _client;
		private readonly ILogger<CreateContainerImageBehavior<TRequest, TResponse>> _logger;

		public CreateContainerImageBehavior(IDockerClient client, ILogger<CreateContainerImageBehavior<TRequest, TResponse>> logger) {
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			_logger.LogDebug("Creating container image {ContainerImage}:{ImageTag}", request.ContainerImage, request.ImageTag);

			var imageCreateParameters = new ImagesCreateParameters() {
				FromImage = request.ContainerImage,
				Tag = request.ImageTag
			};

			var authConfig = new AuthConfig() {
				Email = request.RegistryEmail,
				Username = request.RegistryUsername,
				Password = request.RegistryPassword
			};

			await _client.Images.CreateImageAsync(imageCreateParameters, authConfig, new Progress<JSONMessage>(), cancellationToken);

			return await next();
		}
	}
}
