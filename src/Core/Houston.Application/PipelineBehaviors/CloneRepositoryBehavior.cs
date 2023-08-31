using Houston.Application.CommandHandlers.WorkerCommandHandlers.RunPipeline;

namespace Houston.Application.PipelineBehaviors
{
    public class CloneRepositoryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : WorkerRunPipelineCommand {
		public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken) {
			using var channel = GrpcChannel.ForAddress($"{request.ContainerName}:50051");
			var client = new GitService.GitServiceClient(channel);

			var cloneRepositoryRequest = new CloneRepositoryRequest {
				Url = request.Pipeline.PipelineTrigger.SourceGit,
				Branch = "main",
				PrivateKey = request.Pipeline.PipelineTrigger.PrivateKey
			};
			var cloneRepositoryResponse = await client.CloneRepositoryAsync(cloneRepositoryRequest, cancellationToken: cancellationToken);

			await channel.ShutdownAsync();

			return await next();
		}
	}
}
