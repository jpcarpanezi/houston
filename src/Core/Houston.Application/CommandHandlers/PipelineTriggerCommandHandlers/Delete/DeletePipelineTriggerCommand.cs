namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Delete {
	public sealed record DeletePipelineTriggerCommand(Guid Id) : IRequest<IResultCommand>;
}
