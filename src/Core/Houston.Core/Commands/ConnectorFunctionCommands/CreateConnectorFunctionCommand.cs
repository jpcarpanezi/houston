using Houston.Core.Entities.MongoDB;
using Houston.Core.Enums;
using MediatR;
using MongoDB.Bson;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class CreateConnectorFunctionCommand : IRequest<ResultCommand<ConnectorFunction>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public ObjectId? ConnectorId { get; set; }

		public List<ObjectId>? Dependencies { get; set; }

		public string? Version { get; set; }

		public List<GeneralConnectorFunctionInputCommand>? Inputs { get; set; }

		public List<string> Script { get; set; } = null!;
	}	
}
