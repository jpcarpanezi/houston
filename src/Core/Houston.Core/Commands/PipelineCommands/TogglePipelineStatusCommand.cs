using Houston.Core.Commands;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class TogglePipelineStatusCommand : IRequest<ResultCommand> {
		public Guid Id { get; set; }

		public TogglePipelineStatusCommand() { }

		public TogglePipelineStatusCommand(Guid id) {
			Id = id;
		}
	}
}
