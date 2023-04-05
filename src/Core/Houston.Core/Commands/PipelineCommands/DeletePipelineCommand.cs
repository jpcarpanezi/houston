using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class DeletePipelineCommand : IRequest<ResultCommand> {
		public Guid Id { get; set; }

		public DeletePipelineCommand() { }

		public DeletePipelineCommand(Guid id) {
			Id = id;
		}
	}
}
