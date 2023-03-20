using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;

namespace Houston.Infrastructure.Repository {
	public class PipelineRepository : Repository<Pipeline>, IPipelineRepository {
		public PipelineRepository(IMongoContext context) : base(context) { }

		
	}
}
