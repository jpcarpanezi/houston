using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorCommands {
	public class CreateConnectorCommand : IRequest<ResultCommand<Connector>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public CreateConnectorCommand() { }
		
		public CreateConnectorCommand(string name, string? description) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
		}
	}
}
