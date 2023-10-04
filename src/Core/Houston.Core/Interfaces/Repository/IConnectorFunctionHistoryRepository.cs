namespace Houston.Core.Interfaces.Repository {
	public interface IConnectorFunctionHistoryRepository : IRepository<ConnectorFunctionHistory> {
		Task<ConnectorFunctionHistory?> GetByIdWithInputs(Guid id);

		Task<ConnectorFunctionHistory?> GetActive(Guid id);

		Task<bool> VersionExists(Guid connectorFunctionId, string version);
	}
}
