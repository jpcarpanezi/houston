namespace Houston.Workers.Consumers {
	public class BuildConnectorFunctionConsumer : IConsumer<BuildConnectorFunctionMessage> {
		private readonly IDistributedCache _cache;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMediator _mediator;
		private readonly ILogger<BuildConnectorFunctionConsumer> _logger;

		public BuildConnectorFunctionConsumer(IDistributedCache cache, IUnitOfWork unitOfWork, IMediator mediator, ILogger<BuildConnectorFunctionConsumer> logger) {
			_cache = cache ?? throw new ArgumentNullException(nameof(cache));
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task Consume(ConsumeContext<BuildConnectorFunctionMessage> context) {
			var systemConfiguration = await GetSystemConfiguration();

			var connectorFunction = await GetConnectorFunction(context.Message.ConnectorFunctionId);

			await UpdateBuildStatus(connectorFunction, BuildStatus.Running);

			var command = CreateWorkerBuildConnectorFunctionCommand(systemConfiguration, connectorFunction);

			var response = await ExecuteBuildCommand(command, connectorFunction);

			UpdateConnectorFunctionWithResponse(connectorFunction, response);

			var buildStatus = response.ExitCode == 0 ? BuildStatus.Success : BuildStatus.Failed;
			await UpdateBuildStatus(connectorFunction, buildStatus);
		}

		private async Task<SystemConfiguration> GetSystemConfiguration() {
			var redisConfigurations = await _cache.GetStringAsync("configurations") ?? throw new Exception("Cannot retrieve configurations file from Redis.");
			return JsonSerializer.Deserialize<SystemConfiguration>(redisConfigurations)!;
		}

		private async Task<ConnectorFunction> GetConnectorFunction(Guid connectorFunctionId) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetByIdWithInputs(connectorFunctionId);

			return connectorFunction ?? throw new Exception($"Could not find a connector function with the provided ID: {connectorFunctionId}.");
		}

		private static WorkerBuildConnectorFunctionCommand CreateWorkerBuildConnectorFunctionCommand(SystemConfiguration systemConfiguration, ConnectorFunction connectorFunction) {
			var containerName = $"houston-runner-{Guid.NewGuid()}";

			return new WorkerBuildConnectorFunctionCommand(
				connectorFunction.Script,
				connectorFunction.Package,
				systemConfiguration.ContainerImage,
				systemConfiguration.ImageTag,
				systemConfiguration.RegistryEmail,
				systemConfiguration.RegistryUsername,
				systemConfiguration.RegistryPassword,
				containerName,
				new List<string>()
			);
		}

		private async Task<BuildConnectorFunctionViewModel> ExecuteBuildCommand(WorkerBuildConnectorFunctionCommand command, ConnectorFunction connectorFunction) {
			var response = new BuildConnectorFunctionViewModel();

			try {
				_logger.LogDebug("Executing build command for connector function: {ConnectorFunctionId}.", connectorFunction.Id);
				response = await _mediator.Send(command);
			} catch (Exception ex) {
				var errorMessage = $"An unhandled exception has occurred during the pipeline execution.\nException: {ex}";
				response.ExitCode = 1;
				connectorFunction.BuildStatus = BuildStatus.Failed;
				connectorFunction.BuildStderr = Encoding.ASCII.GetBytes(errorMessage);
				response.Stderr = errorMessage;
			}

			return response;
		}

		private void UpdateConnectorFunctionWithResponse(ConnectorFunction connectorFunction, BuildConnectorFunctionViewModel response) {
			if (response.ExitCode == 0) {
				_logger.LogDebug("Build command for connector function: {ConnectorFunctionId} executed successfully.", connectorFunction.Id);
				connectorFunction.ScriptDist = response.Dist;
				connectorFunction.PackageType = (PackageType)Enum.Parse(typeof(PackageType), response.Type.ToLower(), true);
				connectorFunction.LastUpdate = DateTime.UtcNow;
			} else {
				_logger.LogDebug("Build command for connector function: {ConnectorFunctionId} failed.", connectorFunction.Id);
				connectorFunction.BuildStatus = BuildStatus.Failed;
				connectorFunction.BuildStderr = Encoding.ASCII.GetBytes(response.Stderr ?? "");
			}
		}

		private async Task UpdateBuildStatus(ConnectorFunction connectorFunction, BuildStatus buildStatus) {
			connectorFunction.BuildStatus = buildStatus;

			_unitOfWork.ConnectorFunctionRepository.Update(connectorFunction);
			await _unitOfWork.Commit();
		}
	}
}
