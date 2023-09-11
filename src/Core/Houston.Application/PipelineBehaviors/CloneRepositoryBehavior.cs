namespace Houston.Application.PipelineBehaviors {
	public class CloneRepositoryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : WorkerRunPipelineCommand {
		private readonly ILogger<CloneRepositoryBehavior<TRequest, TResponse>> _logger;

		public CloneRepositoryBehavior(ILogger<CloneRepositoryBehavior<TRequest, TResponse>> logger) {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			_logger.LogDebug("Cloning repository {RepositoryUrl} for pipeline {PipelineId}", request.Pipeline.PipelineTrigger.SourceGit, request.Pipeline.Id);

			using var channel = GrpcChannel.ForAddress($"http://{request.ContainerName}:{request.RunnerPort}", new GrpcChannelOptions {
				HttpHandler = new HttpClientHandler {
					ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
				}
			});

			var client = new GitService.GitServiceClient(channel);

			await Task.Delay(5000, cancellationToken);

			var cloneRepositoryRequest = new CloneRepositoryRequest {
				Url = request.Pipeline.PipelineTrigger.SourceGit,
				Branch = request.Branch,
				PrivateKey = request.Pipeline.PipelineTrigger.PrivateKey
			};
			var cloneRepositoryResponse = await client.CloneRepositoryAsync(cloneRepositoryRequest, cancellationToken: cancellationToken);

			if (cloneRepositoryResponse.HasError) {
				throw new CloneRepositoryException(cloneRepositoryResponse.Stderr ?? "");
			}

			await channel.ShutdownAsync();

			return await next();
		}
	}
}
