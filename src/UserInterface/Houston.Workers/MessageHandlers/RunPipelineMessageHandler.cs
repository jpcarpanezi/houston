using EventBus.EventBus.Abstractions;
using Houston.Core.Entities.Postgres;
using Houston.Core.Entities.Redis;
using Houston.Core.Enums;
using Houston.Core.Exceptions;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Messages;
using Houston.Core.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Diagnostics;
using System.Text.Json;

namespace Houston.Workers.MessageHandlers {
	public class RunPipelineMessageHandler : IIntegrationEventHandler<RunPipelineMessage> {
		private readonly IContainerBuilderParametersService _containerBuilderParameters;
		private readonly IContainerBuilderChainService _containerBuilderChain;
		private readonly IDistributedCache _cache;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<RunPipelineMessageHandler> _logger;

		public RunPipelineMessageHandler(IContainerBuilderParametersService containerBuilder, IContainerBuilderChainService containerChain, IDistributedCache cache, IUnitOfWork unitOfWork, ILogger<RunPipelineMessageHandler> logger) {
			_containerBuilderParameters = containerBuilder ?? throw new ArgumentNullException(nameof(containerBuilder));
			_containerBuilderChain = containerChain ?? throw new ArgumentNullException(nameof(containerChain));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Handle(RunPipelineMessage message) {
			using (LogContext.PushProperty("IntegrationEventContext", $"{message.Id}-{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}")) {
				_logger.LogInformation("Handling integration event: {messageId} at {assemblyName}", message.Id, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);

				var redisConfigurations = await _cache.GetStringAsync("configurations") ?? throw new Exception("Cannot retrieve configurations file from Redis.");
				var configurations = JsonSerializer.Deserialize<SystemConfiguration>(redisConfigurations);

				var pipeline = await _unitOfWork.PipelineRepository.GetActiveWithInverseProperties(message.PipelineId) ?? throw new Exception($"Could not find a pipeline with the provided ID: {message.PipelineId}.");

				if (pipeline.Status != PipelineStatusEnum.Awaiting) {
					_logger.LogInformation("Pipeline with ID {pipelineId} and status {pipelineStatus} was not executed because it was not in awaiting mode.", pipeline.Id, pipeline.Status);
					return;
				}

				await UpdatePipelineStatus(pipeline, PipelineStatusEnum.Running);

				var stopwatch = Stopwatch.StartNew();
				DateTime startTime = DateTime.UtcNow;

				string containerName = $"houston-runner-{Guid.NewGuid()}";
				var builderParameters = _containerBuilderParameters.AddImage(configurations!.ContainerImage, configurations.ImageTag)
							  .AddContainerName(containerName)
							  .AddBind("/var/run/docker.sock:/var/run/docker.sock")
							  .AddInstructions(pipeline.PipelineInstructions.ToList())
							  .AddDeployKey(pipeline.PipelineTrigger.PrivateKey)
							  .AddSourceGit(pipeline.PipelineTrigger.SourceGit)
							  .AddAuthentication(configurations!.RegistryUsername, configurations.RegistryAddress, configurations.RegistryPassword, configurations.RegistryAddress)
							  .Build();

				ContainerChainResponse response = new();
				try {
					response = await _containerBuilderChain.Handler(new ContainerChainResponse(), builderParameters);
				} catch (ContainerBuilderException ex) {
					response.ExitCode = -1;
					response.Stdout = ex.Stdout is null ? string.Empty : ex.Stdout;
					response.InstructionWithError = null;
				} catch (Exception ex) {
					response.ExitCode = -1;
					response.Stdout = $"An unhandled exception has occurred during the pipeline execution.\nException: {ex}";
					response.InstructionWithError = null;
				}

				stopwatch.Stop();

				await UpdatePipelineStatus(pipeline, PipelineStatusEnum.Awaiting);

				var log = new PipelineLog {
					Id = Guid.NewGuid(),
					PipelineId = pipeline.Id,
					ExitCode = response.ExitCode,
					Stdout = response.Stdout,
					InstructionWithError = response.InstructionWithError,
					TriggeredBy = message.TriggeredBy is null ? null : message.TriggeredBy,
					StartTime = startTime,
					Duration = stopwatch.Elapsed
				};

				_unitOfWork.PipelineLogsRepository.Add(log);
				await _unitOfWork.Commit();
			}
		}

		private async Task UpdatePipelineStatus(Pipeline pipeline, PipelineStatusEnum status) {
			pipeline.Status = status;

			_unitOfWork.PipelineRepository.Update(pipeline);
			await _unitOfWork.Commit();
		}
	}
}
