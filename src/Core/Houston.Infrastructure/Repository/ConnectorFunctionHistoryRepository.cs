namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionHistoryRepository : Repository<ConnectorFunctionHistory>, IConnectorFunctionHistoryRepository {
		public ConnectorFunctionHistoryRepository(PostgresContext context) : base(context) { }

		public async Task<ConnectorFunctionHistory?> GetByIdWithInputs(Guid id) {
			return await Context.ConnectorFunctionHistory
								   .Include(x => x.UpdatedByNavigation)
								   .Include(x => x.CreatedByNavigation)
								   .Include(x => x.ConnectorFunctionInputs)
								   .Where(x => x.Id == id)
								   .FirstOrDefaultAsync();
		}

		public async Task<ConnectorFunctionHistory?> GetActive(Guid id) {
			return await Context.ConnectorFunctionHistory.Include(x => x.ConnectorFunction)
												.Where(x => x.Id == id && x.Active && x.ConnectorFunction.Active)
												.FirstOrDefaultAsync();
		}

		public async Task<bool> VersionExists(Guid connectorFunctionId, string version) {
			return await Context.ConnectorFunctionHistory.AnyAsync(x => x.ConnectorFunctionId == connectorFunctionId && x.Version == version);
		}

		public async Task<List<ConnectorFunctionHistory>> GetByIdList(List<Guid> ids) {
			return await Context.ConnectorFunctionHistory.Include(x => x.ConnectorFunctionInputs).Where(x => ids.Contains(x.Id)).ToListAsync();
		}
	}
}
