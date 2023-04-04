using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface IConnectorFunctionRepository : IRepository<ConnectorFunction> {
		Task<ConnectorFunction?> GetByIdWithInputs(Guid id);
	}
}
