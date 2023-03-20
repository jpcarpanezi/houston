using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionRepository : Repository<ConnectorFunction>, IConnectorFunctionRepository {
		public ConnectorFunctionRepository(IMongoContext context) : base(context) { }
	}
}
