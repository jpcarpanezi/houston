using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class PipelineRepository : Repository<Pipeline>, IPipelineRepository {
		public PipelineRepository(PostgresContext context) : base(context) { }

		public async Task<Pipeline?> GetActive(Guid id) {
			return await Context.Pipeline.Include(x => x.CreatedByNavigation)
								.Include(x => x.UpdatedByNavigation)
								.Where(x => x.Id == id && x.Active)
								.FirstOrDefaultAsync();
		}
	}
}
