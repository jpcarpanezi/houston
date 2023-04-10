using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class PipelineTriggerRepository : Repository<PipelineTrigger>, IPipelineTriggerRepository {
		public PipelineTriggerRepository(PostgresContext context) : base(context) { }

		public async Task<bool> AnyPipelineTrigger(Guid pipelineId) {
			return await Context.PipelineTrigger.AnyAsync(x => x.PipelineId == pipelineId);
		}

		public async Task<PipelineTrigger?> GetByIdWithInverseProperties(Guid id) {
			return await Context.PipelineTrigger.Include(x => x.PipelineTriggerEvents)
									   .ThenInclude(x => x.TriggerEvent)
									   .Include(x => x.PipelineTriggerEvents)
									   .ThenInclude(x => x.PipelineTriggerFilters)
									   .ThenInclude(x => x.TriggerFilter)
									   .Where(x => x.Id == id)
									   .FirstOrDefaultAsync();
		}
	}
}
