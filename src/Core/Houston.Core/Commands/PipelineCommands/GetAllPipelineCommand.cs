using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class GetAllPipelineCommand : IRequest<PaginatedResultCommand<Pipeline>> {
		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public GetAllPipelineCommand() { }

		public GetAllPipelineCommand(int pageSize, int pageIndex) {
			PageSize = pageSize;
			PageIndex = pageIndex;
		}
	}
}
