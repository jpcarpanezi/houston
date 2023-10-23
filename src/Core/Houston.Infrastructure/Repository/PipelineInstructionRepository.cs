namespace Houston.Infrastructure.Repository {
	public class PipelineInstructionRepository : Repository<PipelineInstruction>, IPipelineInstructionRepository {
		public PipelineInstructionRepository(PostgresContext context) : base(context) { }

		public async Task<List<PipelineInstruction>> GetByPipelineId(Guid pipelineId) {
			return await Context.PipelineInstruction.Include(x => x.Pipeline)
										   .Include(x => x.ConnectorFunctionHistory)
												.ThenInclude(x => x.ConnectorFunction)
										   .Include(x => x.PipelineInstructionInputs)
										   .OrderBy(x => x.ConnectedToArrayIndex == null ? 0 : 1)
										   .ThenBy(x => x.ConnectedToArrayIndex)
										   .Where(x => x.PipelineId == pipelineId && x.Pipeline.Active)
										   .ToListAsync();
		}
	}
}
