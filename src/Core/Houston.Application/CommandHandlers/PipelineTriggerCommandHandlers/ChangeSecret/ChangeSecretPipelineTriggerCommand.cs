namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.ChangeSecret {
	public sealed record ChangeSecretPipelineTriggerCommand(Guid PipelineTriggerId, string NewSecret) : IRequest<IResultCommand>;
}
