using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionRepository : Repository<ConnectorFunction>, IConnectorFunctionRepository {
		public ConnectorFunctionRepository(PostgresContext context) : base(context) { }

		public async Task<ConnectorFunction?> GetByIdWithInputs(Guid id) {
			return await Context.ConnectorFunction
								   .Include(x => x.ConnectorFunctionInputs)
										.ThenInclude(x => x.UpdatedByNavigation)
								   .Include(x => x.ConnectorFunctionInputs)
										.ThenInclude(x => x.CreatedByNavigation)
								   .Include(x => x.UpdatedByNavigation)
								   .Include(x => x.CreatedByNavigation)
								   .Where(x => x.Id == id)
								   .FirstOrDefaultAsync();
		}
	}
}
