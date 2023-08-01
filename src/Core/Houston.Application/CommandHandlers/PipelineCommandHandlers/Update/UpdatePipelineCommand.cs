namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Update {
	public sealed record UpdatePipelineCommand(Guid Id, string Name, string? Description) : IRequest<IResultCommand>;
}
