using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class CreateConnectorFunctionCommand : IRequest<ResultCommand<ConnectorFunction>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public Guid ConnectorId { get; set; }

		public List<CreateConnectorFunctionInputCommand>? Inputs { get; set; }

		public string[] Script { get; set; } = null!;

		public CreateConnectorFunctionCommand() { }

		public CreateConnectorFunctionCommand(string name, string? description, Guid connectorId, List<CreateConnectorFunctionInputCommand>? inputs, string[] script) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
			ConnectorId = connectorId;
			Inputs = inputs;
			Script = script ?? throw new ArgumentNullException(nameof(script));
		}
	}	
}
