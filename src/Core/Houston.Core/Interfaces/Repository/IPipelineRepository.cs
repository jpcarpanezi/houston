namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineRepository : IRepository<Pipeline> {
		Task<Pipeline?> GetActive(Guid id);

		Task<long> CountActives();

		Task<List<Pipeline>> GetAllActives(int pageSize, int pageIndex);

		Task<Pipeline?> GetActiveWithInverseProperties(Guid id);
	}
}
