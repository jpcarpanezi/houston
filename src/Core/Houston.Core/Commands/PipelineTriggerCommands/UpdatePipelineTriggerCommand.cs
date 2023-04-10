using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class UpdatePipelineTriggerCommand : IRequest<ResultCommand<PipelineTrigger>> {
		public Guid PipelineTriggerId {  get; set; }

		public string SourceGit { get; set; } = null!;

		public List<UpdatePipelineTriggerEvents> Events { get; set; } = new List<UpdatePipelineTriggerEvents>();

		public UpdatePipelineTriggerCommand() { }

		public UpdatePipelineTriggerCommand(Guid pipelineTriggerId, string sourceGit, List<UpdatePipelineTriggerEvents> events) {
			PipelineTriggerId = pipelineTriggerId;
			SourceGit = sourceGit ?? throw new ArgumentNullException(nameof(sourceGit));
			Events = events ?? throw new ArgumentNullException(nameof(events));
		}

		public class UpdatePipelineTriggerEvents {
			public Guid TriggerEventId { get; set; }

			public List<UpdatePipelineTriggerEventFilters> EventFilters { get; set; } = new List<UpdatePipelineTriggerEventFilters>();
		}

		public class UpdatePipelineTriggerEventFilters {
			public Guid TriggerFilterId { get; set; }

			public string[]? FilterValues { get; set; }
		}
	}
}
