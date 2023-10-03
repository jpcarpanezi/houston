namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionRepository : Repository<ConnectorFunction>, IConnectorFunctionRepository {
		public ConnectorFunctionRepository(PostgresContext context) : base(context) { }

		public async Task<long> CountActivesByConnectorId(Guid connectorId) {
			return await Context.ConnectorFunction.Include(x => x.Connector)
										 .Where(x => x.ConnectorId == connectorId && x.Active && x.Connector.Active)
										 .LongCountAsync();
		}

		public async Task<ConnectorFunction?> GetActive(Guid id) {
			return await Context.ConnectorFunction
								   .Include(x => x.UpdatedByNavigation)
								   .Include(x => x.CreatedByNavigation)
								   .Where(x => x.Id == id && x.Active)
								   .FirstOrDefaultAsync();
		}

		public async Task<List<ConnectorFunction>> GetAllActivesByConnectorId(Guid connectorId, int pageSize, int pageIndex) {
			return await Context.ConnectorFunction.Include(x => x.CreatedByNavigation)
								 .Include(x => x.UpdatedByNavigation)
								 .Include(x => x.Connector)
								 .OrderBy(x => x.Name)
								 .Where(x => x.ConnectorId == connectorId && x.Active && x.Connector.Active)
								 .Skip(pageSize * pageIndex)
								 .Take(pageSize)
								 .ToListAsync();
		}

		public async Task<List<ConnectorFunction>> GetByIdList(List<Guid> ids) {
			return await Context.ConnectorFunction.Where(x => ids.Contains(x.Id)).ToListAsync();
		}

		public async Task<ConnectorFunction?> GetByIdWithInputs(Guid id) {
			return await Context.ConnectorFunction
								   .Include(x => x.UpdatedByNavigation)
								   .Include(x => x.CreatedByNavigation)
								   .Where(x => x.Id == id)
								   .FirstOrDefaultAsync();
		}
	}
}
