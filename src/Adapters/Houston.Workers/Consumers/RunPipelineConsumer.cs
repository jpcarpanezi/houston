namespace Houston.Workers.Consumers {
	public class RunPipelineConsumer : IConsumer<RunPipelineMessage> {
		private readonly IDistributedCache _cache;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMediator _mediator;

		public RunPipelineConsumer(IDistributedCache cache, IUnitOfWork unitOfWork, IMediator mediator) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		public async Task Consume(ConsumeContext<RunPipelineMessage> context) {
			var systemConfiguration = await GetSystemConfiguration();

			var pipeline = await _unitOfWork.PipelineRepository.GetActiveWithInverseProperties(context.Message.PipelineId) ?? 
				throw new Exception($"Could not find a pipeline with the provided ID: {context.Message.PipelineId}.");

			await UpdatePipelineStatus(pipeline, PipelineStatus.Running);

			var stopwatch = Stopwatch.StartNew();

			var log = new PipelineLog {
				Id = Guid.NewGuid(),
				PipelineId = pipeline.Id,
				StartTime = DateTime.UtcNow,
				TriggeredBy = context.Message.TriggeredBy,
			};

			try {
				string containerName = $"houston-runner-{Guid.NewGuid()}";
				var address = $"{containerName}:50051";

				var command = new WorkerRunPipelineCommand(
					pipeline, 
					systemConfiguration.ContainerImage, 
					systemConfiguration.ImageTag, 
					systemConfiguration.RegistryEmail, 
					systemConfiguration.RegistryPassword, 
					systemConfiguration.RegistryPassword, 
					containerName, 
					new List<string>()
				);
				var response = await _mediator.Send(command);

				log.ExitCode = response.ExitCode;
				log.Stdout = response.Stdout;
				log.InstructionWithError = response.InstructionWithError;
			} catch (ScriptBuildNotCompleteException ex) {
				log.ExitCode = 1;
				log.Stdout = $"The pipeline could not be executed because the script build of {ex.ConnectorFunctionName} is not complete.";
				log.InstructionWithError = ex.InstructionId;
			} catch (Exception ex) {
				log.ExitCode = 1;
				log.Stdout = $"An unhandled exception has occurred during the pipeline execution.\nException: {ex}";
				log.InstructionWithError = null;
			}

			stopwatch.Stop();

			log.Duration = stopwatch.Elapsed;

			await UpdatePipelineStatus(pipeline, PipelineStatus.Awaiting);

			_unitOfWork.PipelineLogsRepository.Add(log);
			await _unitOfWork.Commit();
		}

		private async Task<SystemConfiguration> GetSystemConfiguration() {
			var redisConfigurations = await _cache.GetStringAsync("configurations") ?? throw new Exception("Cannot retrieve configurations file from Redis.");
			return JsonSerializer.Deserialize<SystemConfiguration>(redisConfigurations)!;
		}

		private async Task UpdatePipelineStatus(Pipeline pipeline, PipelineStatus status) {
			pipeline.Status = status;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();
		}
	}
}
