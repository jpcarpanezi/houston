namespace Houston.Core.Interfaces.Repository {
	public interface IUnitOfWork {
		IUserRepository UserRepository { get; }
		
		IConnectorRepository ConnectorRepository { get; }

		IRepositoryHostRepository RepositoryHostRepository { get; }

		IPipelineRepository PipelineRepository { get; }

		IConnectorFunctionRepository ConnectorFunctionRepository { get; }

		IPipelineLogsRepository PipelineLogsRepository { get; }

		Task<bool> Commit();
	}
}
