namespace Houston.Application.PipelineBehaviors {
	public class StopAndRemoveContainerBehavior<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : ContainerBaseCommand {
		private readonly IDockerClient _client;
		private readonly ILogger<StopAndRemoveContainerBehavior<TRequest, TResponse>> _logger;

		public StopAndRemoveContainerBehavior(IDockerClient client, ILogger<StopAndRemoveContainerBehavior<TRequest, TResponse>> logger) {
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken) {
			_logger.LogInformation("Stopping and removing container {ContainerId}", request.ContainerId);

			if (request.ContainerId is null) return;

			await _client.Containers.StopContainerAsync(request.ContainerId[..12], new ContainerStopParameters(), cancellationToken);
			await _client.Containers.RemoveContainerAsync(request.ContainerId[..12], new ContainerRemoveParameters(), cancellationToken);
		}
	}
}
