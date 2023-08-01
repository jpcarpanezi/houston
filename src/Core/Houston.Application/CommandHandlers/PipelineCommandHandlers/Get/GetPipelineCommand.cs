namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Get {
	public sealed record GetPipelineCommand(Guid Id) : IRequest<IResultCommand>;
}
