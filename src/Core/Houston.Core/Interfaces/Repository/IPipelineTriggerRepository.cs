using Houston.Core.Entities.Postgres;

namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineTriggerRepository : IRepository<PipelineTrigger> {
		Task<bool> AnyPipelineTrigger(Guid pipelineId);

		Task<PipelineTrigger?> GetByIdWithInverseProperties(Guid id);
	}
}
