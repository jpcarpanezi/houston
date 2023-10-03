namespace Houston.Application.CommandHandlers.WorkerCommandHandlers.RunPipeline {
	public class WorkerRunPipelineCommandHandler : IRequestHandler<WorkerRunPipelineCommand, RunPipelineViewModel> {
		private readonly ILogger<WorkerRunPipelineCommandHandler> _logger;

		public WorkerRunPipelineCommandHandler(ILogger<WorkerRunPipelineCommandHandler> logger) {
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<RunPipelineViewModel> Handle(WorkerRunPipelineCommand request, CancellationToken cancellationToken) {
			_logger.LogDebug("Running pipeline {PipelineId} in container {ContainerId}", request.Pipeline.Id, request.ContainerId);

			using var channel = GrpcChannel.ForAddress($"http://{request.ContainerName}:{request.RunnerPort}", new GrpcChannelOptions {
				HttpHandler = new HttpClientHandler {
					ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
				}
			});

			var client = new PipelineService.PipelineServiceClient(channel);

			var files = CompressFiles(request.Pipeline.PipelineInstructions);

			var scripts = request.Pipeline.PipelineInstructions.Select(x => x.Id.ToString()).ToArray();

			await Task.Delay(5000, cancellationToken);

			var runPipelineRequest = new RunPipelineRequest {
				Files = Google.Protobuf.ByteString.CopyFrom(files),
				Scripts = { scripts }
			};
			var runPipelineResponse = await client.RunPipelineAsync(runPipelineRequest, cancellationToken: cancellationToken);

			await channel.ShutdownAsync();

			var stdout = string.Join("\n", runPipelineResponse.Instructions.Select(x => x.Stdout));
			var stderr = string.Join("\n", runPipelineResponse.Instructions.Select(x => x.Stderr));

			var response = new RunPipelineViewModel {
				ExitCode = runPipelineResponse.ExitCode,
				InstructionWithError = runPipelineResponse.Instructions.Where(x => x.HasError == true).Select(x => (Guid?)Guid.Parse(x.Script)).DefaultIfEmpty(null).FirstOrDefault(),
				Stdout = new StringBuilder().AppendLine(stdout).AppendLine(stderr).ToString(),
			};

			return response;
		}

		private static byte[] CompressFiles(ICollection<PipelineInstruction> instructions) {
			using MemoryStream memoryStream = new();
			using (var writer = WriterFactory.Open(memoryStream, ArchiveType.Tar, CompressionType.GZip)) {
				foreach (var instruction in instructions) {
					if (instruction.ConnectorFunctionHistory.PackageType is null || instruction.ConnectorFunctionHistory.ScriptDist is null || instruction.ConnectorFunctionHistory.BuildStatus != BuildStatus.Success) {
						throw new ScriptBuildNotCompleteException(
							"The script could not be compressed because build is not complete.",
							instruction.ConnectorFunctionHistory.BuildStatus,
							instruction.Id,
							instruction.ConnectorFunctionHistoryId,
							instruction.ConnectorFunctionHistory.ConnectorFunction.Name
						);
					}

					var packageJson = new { Type = instruction.ConnectorFunctionHistory.PackageType?.ToString().ToLower() };
					var package = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(packageJson, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));

					writer.Write($"{instruction.Id}/index.js", new MemoryStream(instruction.ConnectorFunctionHistory.ScriptDist), DateTime.Now);
					writer.Write($"{instruction.Id}/package.json", new MemoryStream(package), DateTime.Now);
				}
			}

			memoryStream.Position = 0;
			return memoryStream.ToArray();
		}
	}
}
