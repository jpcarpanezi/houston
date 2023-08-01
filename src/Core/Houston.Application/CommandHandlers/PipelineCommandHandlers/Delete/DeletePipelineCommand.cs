namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Delete {
	public sealed record DeletePipelineCommand(Guid Id) : IRequest<IResultCommand>;
}
