
namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers.Update {
	public sealed record UpdatePipelineTriggerCommand(Guid PipelineTriggerId, string SourceGit) : IRequest<IResultCommand>;
}
