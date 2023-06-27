using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class UpdateDeployKeyCommand : IRequest<ResultCommand<PipelineTrigger>> {
		public Guid PipelineId { get; set; }

		public UpdateDeployKeyCommand() { }

		public UpdateDeployKeyCommand(Guid pipelineId) {
			PipelineId = pipelineId;
		}
	}
}
