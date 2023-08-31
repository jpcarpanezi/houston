namespace Houston.Application.CommandHandlers.WorkerCommandHandlers.RunPipeline {
	public class WorkerRunPipelineCommandHandler : IRequestHandler<WorkerRunPipelineCommand, RunPipelineViewModel> {
		public async Task<RunPipelineViewModel> Handle(WorkerRunPipelineCommand request, CancellationToken cancellationToken) {
			using var channel = GrpcChannel.ForAddress($"{request.ContainerName}:50051");
			var client = new PipelineService.PipelineServiceClient(channel);

			var files = CompressFiles(request.Pipeline.PipelineInstructions);
			var scripts = request.Pipeline.PipelineInstructions.Select(x => x.Id.ToString()).ToArray();

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
				InstructionWithError = runPipelineResponse.Instructions.Where(x => x.HasError == true).Select(x => Guid.Parse(x.Script)).First(),
				Stdout = stdout + "\n" + stderr,
			};

			return response;
		}

		private static byte[] CompressFiles(ICollection<PipelineInstruction> instructions) {
			using MemoryStream memoryStream = new();
			using (var writer = WriterFactory.Open(memoryStream, ArchiveType.Tar, CompressionType.GZip)) {
				foreach (var instruction in instructions) {
					if (instruction.ConnectorFunction.PackageType is null || instruction.ConnectorFunction.ScriptDist is null || instruction.ConnectorFunction.BuildStatus != BuildStatus.Success) {
						throw new ScriptBuildNotCompleteException(
							"The script could not be compressed because build is not complete.",
							instruction.ConnectorFunction.BuildStatus,
							instruction.Id,
							instruction.ConnectorFunctionId,
							instruction.ConnectorFunction.Name
						);
					}

					var package = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(instruction.ConnectorFunction.PackageType?.ToString().ToLower()));

					writer.Write($"{instruction.Id}/index.js", new MemoryStream(instruction.ConnectorFunction.ScriptDist), DateTime.Now);
					writer.Write($"{instruction.Id}/package.json", new MemoryStream(package), DateTime.Now);
				}
			}

			memoryStream.Position = 0;
			return memoryStream.ToArray();
		}
	}
}
