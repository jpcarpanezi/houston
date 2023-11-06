namespace Houston.Core.Interfaces.Repository {
	public interface IUnitOfWork : IDisposable {
		IUserRepository UserRepository { get; }
		
		IConnectorRepository ConnectorRepository { get; }

		IConnectorFunctionHistoryRepository ConnectorFunctionHistoryRepository { get; }

		IConnectorFunctionInputRepository ConnectorFunctionInputRepository { get; }

		IPipelineRepository PipelineRepository { get; }

		IConnectorFunctionRepository ConnectorFunctionRepository { get; }

		IPipelineLogRepository PipelineLogsRepository { get; }

		IPipelineTriggerRepository PipelineTriggerRepository { get; }

		ITriggerEventRepository TriggerEventRepository { get; }

		ITriggerFilterRepository TriggerFilterRepository { get; }

		IPipelineInstructionRepository PipelineInstructionRepository { get; }

		Task<int> Commit();
	}
}
