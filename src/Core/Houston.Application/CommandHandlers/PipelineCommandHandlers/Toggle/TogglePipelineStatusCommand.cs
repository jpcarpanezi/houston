namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Toggle {
	public sealed record TogglePipelineStatusCommand(Guid Id) : IRequest<IResultCommand>;
}
