namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Create {
	public sealed record CreatePipelineTriggerCommand(Guid PipelineId, string SourceGit, string Secret, List<CreatePipelineTriggerEvents> Events) : IRequest<IResultCommand>;

	public sealed record CreatePipelineTriggerEvents(Guid TriggerEventId, List<CreatePipelineTriggerEventFilters> EventFilters);

	public sealed record CreatePipelineTriggerEventFilters(Guid TriggerFilterId, string[]? FilterValues);
}
