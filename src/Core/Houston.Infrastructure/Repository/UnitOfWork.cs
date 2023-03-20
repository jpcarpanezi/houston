using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class UnitOfWork : IUnitOfWork {
		public IUserRepository UserRepository { get; private set; }

		public IConnectorRepository ConnectorRepository { get; private set; }

		public IRepositoryHostRepository RepositoryHostRepository { get; private set; }

		public IPipelineRepository PipelineRepository { get; private set; }

		public IConnectorFunctionRepository ConnectorFunctionRepository { get; private set; }

		public IPipelineLogsRepository PipelineLogsRepository { get; private set; }

		private readonly IMongoContext _context;

		public UnitOfWork(IMongoContext context) {
			_context = context;
			UserRepository = new UserRepository(context);
			ConnectorRepository = new ConnectorRepository(context);
			RepositoryHostRepository = new RepositoryHostRepository(context);
			PipelineRepository = new PipelineRepository(context);
			ConnectorFunctionRepository = new ConnectorFunctionRepository(context);
			PipelineLogsRepository = new PipelineLogsRepository(context);
		}

		public Task<bool> Commit() {
			throw new NotImplementedException();
		}
	}
}
