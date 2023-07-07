using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class RevealPipelineTriggerKeysCommand : IRequest<ResultCommand<PipelineTrigger>> {
		public Guid PipelineId { get; set; }

		public RevealPipelineTriggerKeysCommand(Guid pipelineId) {
			PipelineId = pipelineId;
		}
	}
}
