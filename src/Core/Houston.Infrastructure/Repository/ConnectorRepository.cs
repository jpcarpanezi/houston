using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class ConnectorRepository : Repository<Connector>, IConnectorRepository {
		public ConnectorRepository(PostgresContext context) : base(context) {}

		public Task<Connector?> GetByIdWithInverseProperties(Guid id) {
			return Context.Connector.Include(x => x.CreatedByNavigation).Include(x => x.UpdatedByNavigation).Where(x => x.Id == id).FirstOrDefaultAsync();
		}
	}
}
