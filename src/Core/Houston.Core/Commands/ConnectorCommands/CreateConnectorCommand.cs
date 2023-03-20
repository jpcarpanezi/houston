using Houston.Core.Entities.MongoDB;
using MediatR;

namespace Houston.Core.Commands.ConnectorCommands {
	public class CreateConnectorCommand : IRequest<ResultCommand<Connector>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }
	}
}
