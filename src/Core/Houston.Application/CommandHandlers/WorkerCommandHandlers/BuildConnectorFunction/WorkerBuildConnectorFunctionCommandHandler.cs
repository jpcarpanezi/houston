namespace Houston.Application.CommandHandlers.WorkerCommandHandlers.BuildConnectorFunction {
	public class WorkerBuildConnectorFunctionCommandHandler : IRequestHandler<WorkerBuildConnectorFunctionCommand, BuildConnectorFunctionViewModel> {
		private readonly ILogger<WorkerBuildConnectorFunctionCommandHandler> _logger;

		public WorkerBuildConnectorFunctionCommandHandler(ILogger<WorkerBuildConnectorFunctionCommandHandler> logger) {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<BuildConnectorFunctionViewModel> Handle(WorkerBuildConnectorFunctionCommand request, CancellationToken cancellationToken) {
			_logger.LogInformation("Building connector function in container {ContainerId}", request.ContainerId);
			using var channel = GrpcChannel.ForAddress($"http://{request.ContainerName}:50051", new GrpcChannelOptions {
				HttpHandler = new HttpClientHandler {
					ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
				}
			});

			var client = new PipelineService.PipelineServiceClient(channel);

			var files = CompressFiles(request.Script, request.Package);

			await Task.Delay(5000, cancellationToken);

			var buildConnectorFunctionRequest = new BuildConnectorFunctionRequest {
				Files = Google.Protobuf.ByteString.CopyFrom(files),
			};
			var buildConnectorFunctionResponse = await client.BuildConnectorFunctionAsync(buildConnectorFunctionRequest, new CallOptions().WithWaitForReady());

			await channel.ShutdownAsync();

			var response = new BuildConnectorFunctionViewModel {
				ExitCode = buildConnectorFunctionResponse.ExitCode,
				Stderr = buildConnectorFunctionResponse.Stderr,
				Dist = buildConnectorFunctionResponse.Dist.ToByteArray(),
				Type = buildConnectorFunctionResponse.Type,
			};

			return response; 
		}

		private static byte[] CompressFiles(byte[] script, byte[] package) {
			using MemoryStream memoryStream = new();
			using (var writer = WriterFactory.Open(memoryStream, ArchiveType.Tar, CompressionType.GZip)) {
				writer.Write($"index.js", new MemoryStream(script), DateTime.Now);
				writer.Write($"package.json", new MemoryStream(package), DateTime.Now);
			}

			memoryStream.Position = 0;
			return memoryStream.ToArray();
		}
	}
}
