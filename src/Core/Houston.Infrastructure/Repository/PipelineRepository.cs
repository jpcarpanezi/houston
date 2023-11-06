namespace Houston.Infrastructure.Repository {
	public class PipelineRepository : Repository<Pipeline>, IPipelineRepository {
		public PipelineRepository(PostgresContext context) : base(context) { }

		public async Task<long> CountActives() {
			return await Context.Pipeline.Where(x => x.Active).LongCountAsync();
		}



		public async Task<Pipeline?> GetActive(Guid id) {
			return await Context.Pipeline.Include(x => x.CreatedByNavigation)
								.Include(x => x.UpdatedByNavigation)
								.Where(x => x.Id == id && x.Active)
								.FirstOrDefaultAsync();
		}

		public async Task<Pipeline?> GetActiveWithInverseProperties(Guid id) {
			return await Context.Pipeline.Include(x => x.CreatedByNavigation)
								.Include(x => x.UpdatedByNavigation)
								.Include(x => x.PipelineInstructions.OrderBy(x => x.ConnectedToArrayIndex == null ? 0 : 1).ThenBy(x => x.ConnectedToArrayIndex))
									.ThenInclude(x => x.PipelineInstructionInputs)
										.ThenInclude(x => x.ConnectorFunctionInput)
								.Include(x => x.PipelineInstructions)
									.ThenInclude(x => x.ConnectorFunctionHistory)
								.Include(x => x.PipelineTrigger)
								.Where(x => x.Id == id && x.Active)
								.FirstOrDefaultAsync();
		}

		public async Task<List<Pipeline>> GetAllActives(int pageSize, int pageIndex) {
			return await Context.Pipeline.Include(x => x.CreatedByNavigation)
								 .Include(x => x.UpdatedByNavigation)
								 .OrderBy(x => x.Name)
								 .Where(x => x.Active)
								 .Skip(pageSize * pageIndex)
								 .Take(pageSize)
								 .ToListAsync();
		}
	}
}
