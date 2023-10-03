namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionHistoryRepository : Repository<ConnectorFunctionHistory>, IConnectorFunctionHistoryRepository {
		public ConnectorFunctionHistoryRepository(PostgresContext context) : base(context) { }

		public async Task<ConnectorFunctionHistory?> GetByIdWithInputs(Guid id) {
			return await Context.ConnectorFunctionHistory
								   .Include(x => x.UpdatedByNavigation)
								   .Include(x => x.CreatedByNavigation)
								   .Where(x => x.Id == id)
								   .FirstOrDefaultAsync();
		}
	}
}
