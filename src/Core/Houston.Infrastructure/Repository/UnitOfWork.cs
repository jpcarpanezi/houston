using Houston.Infrastructure.Context;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class UnitOfWork : IUnitOfWork {
		public IUserRepository UserRepository { get; private set; }

		public IConnectorRepository ConnectorRepository { get; private set; }

		public IPipelineRepository PipelineRepository { get; private set; }

		public IConnectorFunctionRepository ConnectorFunctionRepository { get; private set; }

		public IPipelineLogRepository PipelineLogsRepository { get; private set; }

		private readonly PostgresContext _context;

		public UnitOfWork(PostgresContext context) {
			_context = context;
			UserRepository = new UserRepository(context);
			ConnectorRepository = new ConnectorRepository(context);
			PipelineRepository = new PipelineRepository(context);
			ConnectorFunctionRepository = new ConnectorFunctionRepository(context);
			PipelineLogsRepository = new PipelineLogRepository(context);
		}

		public async Task<int> Commit() {
			return await _context.SaveChangesAsync();
		}

		public async void Dispose() {
			await _context.DisposeAsync();
			GC.SuppressFinalize(this);
		}
	}
}
