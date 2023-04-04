using MediatR;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class DeleteConnectorFunctionCommand : IRequest<ResultCommand> {
		public Guid Id { get; set; }

		public DeleteConnectorFunctionCommand() { }

		public DeleteConnectorFunctionCommand(Guid id) {
			Id = id;
		}
	}
}
