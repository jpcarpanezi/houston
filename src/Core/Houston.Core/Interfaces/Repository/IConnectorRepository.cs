namespace Houston.Core.Interfaces.Repository {
	public interface IConnectorRepository : IRepository<Connector> {
		Task<Connector?> GetByIdWithInverseProperties(Guid id);

		Task<Connector?> GetActive(Guid id);

		Task<long> CountActives();

		Task<List<Connector>> GetAllActives(int pageSize, int pageIndex);
	}
}
