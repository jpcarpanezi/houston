using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineInstructionCommands {
	public class GetAllPipelineInstructionCommand : IRequest<ResultCommand<List<PipelineInstruction>>> {
		public Guid PipelineId { get; set; }

		public GetAllPipelineInstructionCommand(Guid pipelineId) {
			PipelineId = pipelineId;
		}
	}
}
