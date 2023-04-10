using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface ITriggerEventRepository : IRepository<TriggerEvent> {
		Task<long> CountByIdList(List<Guid> ids);
	}
}
