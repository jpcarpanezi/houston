namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Get {
	public sealed record GetPipelineTriggerCommand(Guid PipelineId) : IRequest<IResultCommand>;
}
