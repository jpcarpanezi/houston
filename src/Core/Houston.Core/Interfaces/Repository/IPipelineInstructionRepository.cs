namespace Houston.Core.Interfaces.Repository {
	public interface IPipelineInstructionRepository : IRepository<PipelineInstruction> {
		Task<List<PipelineInstruction>> GetByPipelineId(Guid pipelineId);
	}
}
