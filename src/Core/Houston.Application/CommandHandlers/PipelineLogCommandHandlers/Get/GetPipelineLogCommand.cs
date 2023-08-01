namespace Houston.Application.CommandHandlers.PipelineLogCommandHandlers.Get {
	public sealed record GetPipelineLogCommand(Guid Id) : IRequest<IResultCommand>;
}
