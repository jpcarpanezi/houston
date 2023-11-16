namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public sealed record CreatePipelineTriggerCommand(Guid PipelineId, string SourceGit, string Secret) : IRequest<IResultCommand>;
}
