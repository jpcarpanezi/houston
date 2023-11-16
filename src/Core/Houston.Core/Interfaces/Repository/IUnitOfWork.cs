namespace Houston.Core.Interfaces.Repository {
	public interface IUnitOfWork : IDisposable {
		IUserRepository UserRepository { get; }
		
		IConnectorRepository ConnectorRepository { get; }

		IPipelineRepository PipelineRepository { get; }

		IConnectorFunctionRepository ConnectorFunctionRepository { get; }

		IPipelineLogRepository PipelineLogsRepository { get; }

		IPipelineTriggerRepository PipelineTriggerRepository { get; }

		Task<int> Commit();
	}
}
