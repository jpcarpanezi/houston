namespace Houston.Core.Interfaces.Repository {
	public interface IConnectorFunctionRepository : IRepository<ConnectorFunction> {
		Task<bool> AnyActive(Guid connectorId, string name, string version);
		
		Task<ConnectorFunction?> GetActive(Guid id);

		Task<long> CountActivesByConnectorId(Guid connectorId);

		Task<List<ConnectorFunction>> GetAllActivesByConnectorId(Guid connectorId, int pageSize, int pageIndex);

		Task<ConnectorFunction?> GetByIdWithInputs(Guid id);

		Task<List<ConnectorFunction>> GetByIdList(List<Guid> ids);
	}
}
