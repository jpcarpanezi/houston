namespace Houston.Core.Interfaces.Repository {
	public interface IConnectorFunctionHistoryRepository : IRepository<ConnectorFunctionHistory> {
		Task<ConnectorFunctionHistory?> GetByIdWithInputs(Guid id);
	}
}
