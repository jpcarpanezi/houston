using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineLogRepository : IRepository<PipelineLog> {
		Task<double> DurationAverage(Guid pipelineId, int pageSize = 25);
	}
}
