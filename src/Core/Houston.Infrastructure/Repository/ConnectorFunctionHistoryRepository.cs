namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionHistoryRepository : Repository<ConnectorFunctionHistory>, IConnectorFunctionHistoryRepository {
		public ConnectorFunctionHistoryRepository(PostgresContext context) : base(context) { }
	}
}
