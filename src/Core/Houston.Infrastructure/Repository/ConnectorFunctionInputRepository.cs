namespace Houston.Infrastructure.Repository {
	public class ConnectorFunctionInputRepository : Repository<ConnectorFunctionInput>, IConnectorFunctionInputRepository {
		public ConnectorFunctionInputRepository(PostgresContext context) : base(context) { }
	}
}
