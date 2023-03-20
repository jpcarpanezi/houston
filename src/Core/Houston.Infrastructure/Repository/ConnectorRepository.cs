﻿using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class ConnectorRepository : Repository<Connector>, IConnectorRepository {
		public ConnectorRepository(IMongoContext context) : base(context) {}
	}
}
