using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class PipelineLogRepository : Repository<PipelineLog>, IPipelineLogRepository {
		public PipelineLogRepository(PostgresContext context) : base(context) { }

		public async Task<double> DurationAverage(Guid pipelineId, int pageSize = 25) {
			var logs = await Context.PipelineLog.Where(x => x.PipelineId == pipelineId)
								   .OrderByDescending(x => x.StartTime)
								   .Take(pageSize)
								   .ToListAsync();

			return logs.Select(x => x.Duration.Ticks).DefaultIfEmpty(0).Average();
		}
	}
}
