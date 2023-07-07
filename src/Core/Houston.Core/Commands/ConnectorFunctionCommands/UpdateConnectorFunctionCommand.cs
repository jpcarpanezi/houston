using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class UpdateConnectorFunctionCommand : IRequest<ResultCommand<ConnectorFunction>> {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public List<UpdateConnectorFunctionInputCommand>? Inputs { get; set; }

		public string[] Script { get; set; } = null!;

		public UpdateConnectorFunctionCommand() { }

		public UpdateConnectorFunctionCommand(Guid connectorFunctionId, string name, string? description, List<UpdateConnectorFunctionInputCommand>? inputs, string[] script) {
			Id = connectorFunctionId;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
			Inputs = inputs;
			Script = script ?? throw new ArgumentNullException(nameof(script));
		}
	}
}
