using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Houston.Infrastructure.Repository {
	public class PipelineInstructionRepository : Repository<PipelineInstruction>, IPipelineInstructionRepository {
		public PipelineInstructionRepository(PostgresContext context) : base(context) { }

		public async Task<List<PipelineInstruction>> GetByPipelineId(Guid pipelineId) {
			return await Context.PipelineInstruction.Include(x => x.Pipeline)
										   .Include(x => x.PipelineInstructionInputs)
										   .OrderByDescending(x => x.ConnectedToArrayIndex)
										   .Where(x => x.PipelineId == pipelineId && x.Pipeline.Active)
										   .ToListAsync();
		}
	}
}
