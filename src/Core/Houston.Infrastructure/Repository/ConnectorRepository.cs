﻿using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class ConnectorRepository : Repository<Connector>, IConnectorRepository {
		public ConnectorRepository(PostgresContext context) : base(context) {}
	}
}
