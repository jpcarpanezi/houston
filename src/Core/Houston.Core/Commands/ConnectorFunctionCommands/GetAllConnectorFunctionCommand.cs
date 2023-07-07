using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorFunctionCommands {
	public class GetAllConnectorFunctionCommand : IRequest<PaginatedResultCommand<ConnectorFunction>> {
		public Guid ConnectorId { get; set; }

		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public GetAllConnectorFunctionCommand() { }

		public GetAllConnectorFunctionCommand(Guid connectorId, int pageSize, int pageIndex) {
			ConnectorId = connectorId;
			PageSize = pageSize;
			PageIndex = pageIndex;
		}
	}
}
