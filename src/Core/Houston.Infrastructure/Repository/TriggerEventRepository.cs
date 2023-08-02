namespace Houston.Infrastructure.Repository {
	public class TriggerEventRepository : Repository<TriggerEvent>, ITriggerEventRepository {
		public TriggerEventRepository(PostgresContext context) : base(context) { }

		public async Task<long> CountByIdList(List<Guid> ids) {
			return await Context.TriggerEvent.Where(x => ids.Contains(x.Id)).LongCountAsync();
		}
	}
}
