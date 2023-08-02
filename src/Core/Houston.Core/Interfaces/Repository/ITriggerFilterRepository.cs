namespace Houston.Core.Interfaces.Repository {
	public interface ITriggerFilterRepository : IRepository<TriggerFilter> {
		Task<long> CountByIdList(List<Guid> ids);
	}
}
