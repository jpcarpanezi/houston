using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class DeletePipelineTriggerCommand : IRequest<ResultCommand> {
		public Guid Id { get; set; }

		public DeletePipelineTriggerCommand(Guid id) {
			Id = id;
		}
	}
}
