namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Run {
	public sealed record RunPipelineCommand(Guid Id) : IRequest<IResultCommand>;
}
