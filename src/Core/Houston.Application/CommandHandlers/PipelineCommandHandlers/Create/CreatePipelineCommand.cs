namespace Houston.Application.CommandHandlers.PipelineCommandHandlers.Create {
	public sealed record CreatePipelineCommand(string Name, string? Description) : IRequest<IResultCommand>;
}
