using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionInputRepository : Repository<ConnectorFunctionInput>, IConnectorFunctionInputRepository {
		public ConnectorFunctionInputRepository(PostgresContext context) : base(context) { }
	}
}
