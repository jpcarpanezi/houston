using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Infrastructure.Context;

namespace Houston.Infrastructure.Repository {
	public class PipelineRepository : Repository<Pipeline>, IPipelineRepository {
		public PipelineRepository(PostgresContext context) : base(context) { }

		
	}
}
