
namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public sealed record UpdatePipelineTriggerCommand(Guid PipelineTriggerId, string SourceGit, List<UpdatePipelineTriggerEvents> Events) : IRequest<IResultCommand>;

	public sealed record UpdatePipelineTriggerEvents(Guid TriggerEventId, List<UpdatePipelineTriggerEventFilters> EventFilters);

	public sealed record UpdatePipelineTriggerEventFilters(Guid TriggerFilterId, string[]? FilterValues);
}
