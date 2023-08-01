namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.RevealKeys {
	public sealed record RevealPipelineTriggerKeysCommand(Guid PipelineId) : IRequest<IResultCommand>;
}
