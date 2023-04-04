using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class GetConnectorFunctionCommand : IRequest<ResultCommand<ConnectorFunction>> {
		public Guid Id { get; set; }

		public GetConnectorFunctionCommand() { }

		public GetConnectorFunctionCommand(Guid id) {
			Id = id;
		}
	}
}
