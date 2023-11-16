namespace Houston.Infrastructure.Repository {
	public class ConnectorRepository : Repository<Connector>, IConnectorRepository {
		public ConnectorRepository(PostgresContext context) : base(context) {}

		public async Task<long> CountActives() {
			return await Context.Connector.Where(x => x.Active).LongCountAsync();
		}

		public async Task<Connector?> GetActiveByName(string name) {
			return await Context.Connector
								.Where(x => x.Active && x.Name == name)
								.FirstOrDefaultAsync();
		}

		public async Task<Connector?> GetActive(Guid id) {
			return await Context.Connector.Include(x => x.CreatedByNavigation)
						   .Include(x => x.UpdatedByNavigation)
						   .Where(x => x.Id == id && x.Active)
						   .FirstOrDefaultAsync();
		}

		public async Task<List<Connector>> GetAllActives(int pageSize, int pageIndex) {
			return await Context.Connector.Include(x => x.CreatedByNavigation)
								 .Include(x => x.UpdatedByNavigation)
								 .OrderBy(x => x.Name)
								 .Where(x => x.Active)
								 .Skip(pageSize * pageIndex)
								 .Take(pageSize)
								 .ToListAsync();
		}

		public async Task<Connector?> GetByIdWithInverseProperties(Guid id) {
			return await Context.Connector.Include(x => x.CreatedByNavigation)
						   .Include(x => x.UpdatedByNavigation)
						   .Where(x => x.Id == id)
						   .FirstOrDefaultAsync();
		}
	}
}
