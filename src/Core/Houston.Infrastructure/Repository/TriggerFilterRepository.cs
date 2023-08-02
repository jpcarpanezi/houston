namespace Houston.Infrastructure.Repository {
	public class TriggerFilterRepository : Repository<TriggerFilter>, ITriggerFilterRepository {
		public TriggerFilterRepository(PostgresContext context) : base(context) { }

		public async Task<long> CountByIdList(List<Guid> ids) {
			return await Context.TriggerFilter.Where(x => ids.Contains(x.Id)).LongCountAsync();
		}
	}
}
