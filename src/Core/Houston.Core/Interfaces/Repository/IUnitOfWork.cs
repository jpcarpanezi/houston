namespace Houston.Core.Interfaces.Repository {
	public interface IUnitOfWork : IDisposable {
		IUserRepository UserRepository { get; }
		
		IConnectorRepository ConnectorRepository { get; }

		IConnectorFunctionInputRepository ConnectorFunctionInputRepository { get; }

		IPipelineRepository PipelineRepository { get; }

		IConnectorFunctionRepository ConnectorFunctionRepository { get; }

		IPipelineLogRepository PipelineLogsRepository { get; }

		Task<int> Commit();
	}
}
