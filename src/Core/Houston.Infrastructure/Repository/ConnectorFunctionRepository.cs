using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionRepository : Repository<ConnectorFunction>, IConnectorFunctionRepository {
		public ConnectorFunctionRepository(PostgresContext context) : base(context) { }
	}
}
