namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.UpdateKey {
	public sealed record UpdateDeployKeyCommand(Guid PipelineId) : IRequest<IResultCommand>;
}
