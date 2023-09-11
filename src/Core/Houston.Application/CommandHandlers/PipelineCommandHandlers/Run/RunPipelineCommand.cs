namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Run {
	public sealed record RunPipelineCommand(Guid Id, string Branch) : IRequest<IResultCommand>;
}
