using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class PipelineLogsRepository : Repository<PipelineLogs>, IPipelineLogsRepository {
		public PipelineLogsRepository(IMongoContext context) : base(context) { }
	}
}
