using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class RunPipelineCommand : IRequest<ResultCommand> {
		public Guid Id { get; set; }

		public RunPipelineCommand() { }

		public RunPipelineCommand(Guid id) {
			Id = id;
		}
	}
}
