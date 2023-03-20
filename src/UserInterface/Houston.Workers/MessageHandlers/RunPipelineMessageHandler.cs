using EventBus.EventBus.Abstractions;
using Houston.Core.Entities.MongoDB;
using Houston.Core.Entities.Redis;
using Houston.Core.Enums;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Core.Messages;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Serilog.Context;
using System.Diagnostics;
using System.Text.Json;

namespace Houston.Workers.MessageHandlers {
	public class RunPipelineMessageHandler : IIntegrationEventHandler<RunPipelineMessage> {
		private readonly IContainerBuilderService _containerBuilder;
		private readonly IDistributedCache _cache;
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger<RunPipelineMessageHandler> _logger;

		public RunPipelineMessageHandler(IContainerBuilderService containerBuilder, IDistributedCache cache, IUnitOfWork unitOfWork, ILogger<RunPipelineMessageHandler> logger) {
			_containerBuilder = containerBuilder ?? throw new ArgumentNullException(nameof(containerBuilder));
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Handle(RunPipelineMessage message) {
			using (LogContext.PushProperty("IntegrationEventContext", $"{message.Id}-{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}")) {
				_logger.LogInformation($"Handling integration event: {message.Id} at {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}");

				var redisConfigurations = await _cache.GetStringAsync("configurations") ?? throw new ArgumentNullException("Cannot retrieve configurations file from Redis.");
				var configurations = JsonSerializer.Deserialize<SystemConfiguration>(redisConfigurations);

				var pipeline = await _unitOfWork.PipelineRepository.FindByIdAsync(ObjectId.Parse(message.PipelineId));
				if (pipeline is null) {
					throw new ArgumentNullException(nameof(pipeline));
				}

				if (pipeline.PipelineStatus != PipelineStatusEnum.Awaiting) {

				}

				await UpdatePipelineStatus(pipeline.Id, PipelineStatusEnum.Running);

				var stopwatch = Stopwatch.StartNew();
				DateTime startTime = DateTime.UtcNow;

				string containerName = $"houston-runner-{Guid.NewGuid()}";
				var response = await _containerBuilder.PerformAuth(configurations!.RegistryUsername, configurations.RegistryAddress, configurations.RegistryPassword, configurations.RegistryAddress)
							  .AddImage(configurations.ContainerImage, configurations.ImageTag)
							  .AddContainer(containerName, new List<string> { "/var/run/docker.sock:/var/run/docker.sock" })
							  .FromPipeline(pipeline)
							  .Build();

				stopwatch.Stop();

				await UpdatePipelineStatus(pipeline.Id, PipelineStatusEnum.Awaiting);

				var log = new PipelineLogs {
					Id = ObjectId.GenerateNewId(),
					PipelineId = pipeline.Id,
					ExitCode = response.ExitCode,
					Stdout = response.Stdout,
					InstructionWithError = response.InstructionWithError,
					TriggeredBy = message.TriggeredBy is null ? null : ObjectId.Parse(message.TriggeredBy),
					StartTime = startTime,
					Duration = stopwatch.Elapsed
				};

				await _unitOfWork.PipelineLogsRepository.InsertOneAsync(log);
			}
		}

		private async Task UpdatePipelineStatus(ObjectId pipelineId, PipelineStatusEnum status) {
			await _unitOfWork.PipelineRepository.UpdateOneAsync(x => x.Id, pipelineId, x => x.PipelineStatus, status);
		}
	}
}
