using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.ConnectorCommands {
	public class GetAllConnectorCommand : IRequest<PaginatedResultCommand<Connector>> {
		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public GetAllConnectorCommand() { }

		public GetAllConnectorCommand(int pageSize, int pageIndex) {
			PageSize = pageSize;
			PageIndex = pageIndex;
		}
	}
}
