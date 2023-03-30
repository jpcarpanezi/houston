using Houston.Infrastructure.Context;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class PipelineLogRepository : Repository<PipelineLog>, IPipelineLogRepository {
		public PipelineLogRepository(PostgresContext context) : base(context) { }
	}
}
