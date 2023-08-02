namespace Houston.Infrastructure.Repository {
	public class UnitOfWork : IUnitOfWork {
		public IUserRepository UserRepository { get; private set; }

		public IConnectorRepository ConnectorRepository { get; private set; }

		public IConnectorFunctionInputRepository ConnectorFunctionInputRepository { get; private set; }

		public IPipelineRepository PipelineRepository { get; private set; }

		public IConnectorFunctionRepository ConnectorFunctionRepository { get; private set; }

		public IPipelineLogRepository PipelineLogsRepository { get; private set; }

		public IPipelineTriggerRepository PipelineTriggerRepository { get; private set; }

		public ITriggerEventRepository TriggerEventRepository { get; private set; }

		public ITriggerFilterRepository TriggerFilterRepository { get; private set; }

		public IPipelineInstructionRepository PipelineInstructionRepository { get; private set; }

		private readonly PostgresContext _context;

		public UnitOfWork(PostgresContext context) {
			_context = context;
			UserRepository = new UserRepository(context);
			ConnectorRepository = new ConnectorRepository(context);
			PipelineRepository = new PipelineRepository(context);
			ConnectorFunctionRepository = new ConnectorFunctionRepository(context);
			PipelineLogsRepository = new PipelineLogRepository(context);
			ConnectorFunctionInputRepository = new ConnectorFunctionInputRepository(context);
			PipelineTriggerRepository = new PipelineTriggerRepository(context);
			TriggerEventRepository = new TriggerEventRepository(context);
			TriggerFilterRepository = new TriggerFilterRepository(context);
			PipelineInstructionRepository = new PipelineInstructionRepository(context);
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
