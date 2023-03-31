using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface IConnectorRepository : IRepository<Connector> {
		Task<Connector?> GetByIdWithInverseProperties(Guid id);

		Task<Connector?> GetActive(Guid id);
	}
}
