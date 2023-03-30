using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class CreateConnectorFunctionCommand : IRequest<ResultCommand<ConnectorFunction>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public Guid? ConnectorId { get; set; }

		public List<Guid>? Dependencies { get; set; }

		public string? Version { get; set; }

		public List<GeneralConnectorFunctionInputCommand>? Inputs { get; set; }

		public List<string> Script { get; set; } = null!;
	}	
}
