using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorCommands {
	public class UpdateConnectorCommand : IRequest<ResultCommand<Connector>> {
		public Guid ConnectorId { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public UpdateConnectorCommand() { }

		public UpdateConnectorCommand(Guid connectorId, string name, string? description) {
			ConnectorId = connectorId;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
		}
	}
}
