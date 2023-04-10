using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class ChangeSecretPipelineTriggerCommand : IRequest<ResultCommand> {
		public Guid PipelineTriggerId { get; set; }

		public string NewSecret { get; set; } = null!;
	}
}
