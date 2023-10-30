namespace Houston.Workers.Consumers {
	public class RunPipelineConsumer : IConsumer<RunPipelineMessage> {
		private readonly IDistributedCache _cache;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMediator _mediator;
		private readonly ILogger<RunPipelineConsumer> _logger;

		public RunPipelineConsumer(IDistributedCache cache, IUnitOfWork unitOfWork, IMediator mediator, ILogger<RunPipelineConsumer> logger) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<RunPipelineMessage> context) {
			var systemConfiguration = await GetSystemConfiguration();

			var pipeline = await GetPipeline(context.Message.PipelineId);

			var inputs = CreateInputsList(pipeline);

			await UpdatePipelineStatus(pipeline, PipelineStatus.Running);

			var log = CreatePipelineLog(pipeline, context.Message.TriggeredBy);

			try {
				_logger.LogDebug("Running pipeline {PipelineId}", pipeline.Id);
				
				var command = CreateWorkerRunPipelineCommand(pipeline, context.Message.Branch, systemConfiguration, inputs);
				var response = await _mediator.Send(command);

				_logger.LogDebug("Pipeline {PipelineId} finished with exit code {ExitCode}.", pipeline.Id, response.ExitCode);

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

			log.Duration = DateTime.UtcNow - log.StartTime;

			await UpdatePipelineStatus(pipeline, PipelineStatus.Awaiting);

			_unitOfWork.PipelineLogsRepository.Add(log);
			await _unitOfWork.Commit();
		}

		private async Task<SystemConfiguration> GetSystemConfiguration() {
			var redisConfigurations = await _cache.GetStringAsync("configurations") ?? throw new Exception("Cannot retrieve configurations file from Redis.");
			return JsonSerializer.Deserialize<SystemConfiguration>(redisConfigurations)!;
		}

		private async Task<Pipeline> GetPipeline(Guid pipelineId) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActiveWithInverseProperties(pipelineId);

			return pipeline ?? throw new Exception($"Could not find a pipeline with the provided ID: {pipelineId}.");
		}

		private static PipelineLog CreatePipelineLog(Pipeline pipeline, Guid? triggeredBy) {
			return new PipelineLog {
				Id = Guid.NewGuid(),
				PipelineId = pipeline.Id,
				StartTime = DateTime.UtcNow,
				TriggeredBy = triggeredBy,
			};
		}
		
		private static WorkerRunPipelineCommand CreateWorkerRunPipelineCommand(Pipeline pipeline, string branch, SystemConfiguration systemConfiguration, List<string> envs) {
			string containerName = $"houston-runner-{Guid.NewGuid()}";

			return new WorkerRunPipelineCommand(
				pipeline,
				branch,
				systemConfiguration.ContainerImage,
				systemConfiguration.ImageTag,
				systemConfiguration.RegistryEmail,
				systemConfiguration.RegistryPassword,
				systemConfiguration.RegistryPassword,
				containerName,
				envs
			);
		}

		private static List<string> CreateInputsList(Pipeline pipeline) {
			var inputs = new List<string>();

			foreach (var instruction in pipeline.PipelineInstructions) {
				foreach (var input in instruction.PipelineInstructionInputs) {
					if (string.IsNullOrEmpty(input.ConnectorFunctionInput.Replace)) {
						continue;
					}

					var env = new StringBuilder().Append("INPUT_")
								  .Append(input.ConnectorFunctionInput.Replace)
								  .Append('=')
								  .Append(input.ReplaceValue)
								  .ToString();

					inputs.Add(env);
				}
			}

			return inputs;
		}


		private async Task UpdatePipelineStatus(Pipeline pipeline, PipelineStatus status) {
			pipeline.Status = status;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();
		}
	}
}
