using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorCommands {
	public class GetConnectorCommand : IRequest<ResultCommand<Connector>> {
		public Guid ConnectorId { get; set; }

		public GetConnectorCommand(Guid connectorId) {
			ConnectorId = connectorId;
		}
	}
}
