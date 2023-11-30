namespace Houston.Application.PipelineBehaviors {
	public class StopAndRemoveContainerOnExceptionBehavior<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException> where TRequest : ContainerBaseCommand where TException : Exception {
		private readonly IDockerClient _client;
		private readonly ILogger<StopAndRemoveContainerOnExceptionBehavior<TRequest, TResponse, TException>> _logger;

		public StopAndRemoveContainerOnExceptionBehavior(IDockerClient client, ILogger<StopAndRemoveContainerOnExceptionBehavior<TRequest, TResponse, TException>> logger) {
			_client = client ?? throw new ArgumentNullException(nameof(client));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken) {
			_logger.LogDebug(exception, "Stopping and removing container {ContainerId}", request.ContainerId);

			if (request.ContainerId is null) return;

			await _client.Containers.StopContainerAsync(request.ContainerId[..12], new ContainerStopParameters(), cancellationToken);
			await _client.Containers.RemoveContainerAsync(request.ContainerId[..12], new ContainerRemoveParameters(), cancellationToken);
		}
	}
}
