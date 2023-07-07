using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class PipelineLogRepository : Repository<PipelineLog>, IPipelineLogRepository {
		public PipelineLogRepository(PostgresContext context) : base(context) { }

		public async Task<long> CountByPipelineId(Guid pipelineId) {
			return await Context.PipelineLog.Where(x => x.PipelineId == pipelineId).LongCountAsync();
		}

		public async Task<double> DurationAverage(Guid pipelineId, int pageSize = 25) {
			var logs = await Context.PipelineLog.Where(x => x.PipelineId == pipelineId)
								   .OrderByDescending(x => x.StartTime)
								   .Take(pageSize)
								   .ToListAsync();

			return logs.Select(x => x.Duration.Ticks).DefaultIfEmpty(0).Average();
		}

		public async Task<List<PipelineLog>> GetAllByPipelineId(Guid pipelineId, int pageSize, int pageIndex) {
			return await Context.PipelineLog.Include(x => x.TriggeredByNavigation)
								   .Include(x => x.PipelineInstruction)
								   .ThenInclude(x => x.ConnectorFunction)
								   .Where(x => x.PipelineId == pipelineId)
								   .OrderByDescending(x => x.StartTime)
								   .Skip(pageSize * pageIndex)
								   .Take(pageSize)
								   .ToListAsync();
		}

		public async Task<PipelineLog?> GetByIdWithInverseProperties(Guid id) {
			return await Context.PipelineLog.Include(x => x.TriggeredByNavigation)
								   .Include(x => x.PipelineInstruction)
								   .ThenInclude(x => x.ConnectorFunction)
								   .Where(x => x.Id == id)
								   .FirstOrDefaultAsync();
		}
	}
}
