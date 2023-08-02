namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineTriggerRepository : IRepository<PipelineTrigger> {
		Task<bool> AnyPipelineTrigger(Guid pipelineId);

		Task<PipelineTrigger?> GetByIdWithInverseProperties(Guid id);

		Task<PipelineTrigger?> GetByPipelineId(Guid pipelineId);
	}
}
