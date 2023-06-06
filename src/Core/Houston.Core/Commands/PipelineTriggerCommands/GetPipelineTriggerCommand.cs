using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class GetPipelineTriggerCommand : IRequest<ResultCommand<PipelineTrigger>> {
		public Guid PipelineId { get; set; }

		public GetPipelineTriggerCommand(Guid pipelineId) {
			PipelineId = pipelineId;
		}
	}
}
