using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class CreatePipelineTriggerCommand : IRequest<ResultCommand<PipelineTrigger>> {
		public Guid PipelineId { get; set; }

		public string SourceGit { get; set; } = null!;

		public string Secret { get; set; } = null!;

		public List<CreatePipelineTriggerEvents> Events { get; set; } = new List<CreatePipelineTriggerEvents>();

		public CreatePipelineTriggerCommand() { }

		public CreatePipelineTriggerCommand(Guid pipelineId, string sourceGit, string secret, List<CreatePipelineTriggerEvents> events) {
			PipelineId = pipelineId;
			SourceGit = sourceGit ?? throw new ArgumentNullException(nameof(sourceGit));
			Secret = secret ?? throw new ArgumentNullException(nameof(secret));
			Events = events ?? throw new ArgumentNullException(nameof(events));
		}

		public class CreatePipelineTriggerEvents {
			public Guid TriggerEventId { get; set; }

			public List<CreatePipelineTriggerEventFilters> EventFilters { get; set; } = new List<CreatePipelineTriggerEventFilters>();
		}

		public class CreatePipelineTriggerEventFilters {
			public Guid TriggerFilterId { get; set; }

			public string[]? FilterValues { get; set; }
		}
	}
}
