namespace Houston.Application.PipelineBehaviors {
	public class CreateContainerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ContainerBaseCommand {
		private readonly IDockerClient _client;
		private readonly IConfiguration _configuration;
		private readonly ILogger<CreateContainerBehavior<TRequest, TResponse>> _logger;

		public CreateContainerBehavior(IDockerClient client, IConfiguration configuration, ILogger<CreateContainerBehavior<TRequest, TResponse>> logger) {
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			_logger.LogDebug("Creating container {ContainerName} from image {ContainerImage}:{ImageTag}", request.ContainerName, request.ContainerImage, request.ImageTag);

			var networkMode = _configuration["NetworkMode"] ?? "houston";

			int randomPort = Random.Shared.Next(50000, 50100);
			request.Env.Add($"PORT={randomPort}");
			request.RunnerPort = randomPort.ToString();

			var container = await _client.Containers.CreateContainerAsync(new CreateContainerParameters {
				Image = $"{request.ContainerImage}:{request.ImageTag}",
				Name = request.ContainerName,
				Tty = true,
				AttachStdout = true,
				AttachStderr = true,
				AttachStdin = true,
				Env = request.Env,
				HostConfig = new HostConfig {
					Privileged = true,
					NetworkMode = networkMode
				}
			}, cancellationToken);

			request.ContainerId = container.ID;

			await _client.Containers.StartContainerAsync(container.ID[..12], new ContainerStartParameters(), cancellationToken);

			return await next(); 
		}
	}
}
