using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineLogRepository : IRepository<PipelineLog> {
		Task<double> DurationAverage(Guid pipelineId, int pageSize = 25);

		Task<PipelineLog?> GetByIdWithInverseProperties(Guid id);

		Task<List<PipelineLog>> GetAllByPipelineId(Guid pipelineId, int pageSize, int pageIndex);

		Task<long> CountByPipelineId(Guid pipelineId);
	}
}
