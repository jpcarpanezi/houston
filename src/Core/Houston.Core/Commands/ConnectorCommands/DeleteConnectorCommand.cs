using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorCommands {
	public class DeleteConnectorCommand : IRequest<ResultCommand> {
		public Guid ConnectorId { get; set; }

		public DeleteConnectorCommand() { }

		public DeleteConnectorCommand(Guid connectorId) {
			ConnectorId = connectorId;
		}
	}
}
