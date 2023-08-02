namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.GetAll {
	public sealed record GetAllPipelineInstructionCommand(Guid PipelineId) : IRequest<IResultCommand>;
}
