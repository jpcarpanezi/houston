using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineLogCommands {
	public class GetAllPipelineLogCommand : IRequest<PaginatedResultCommand<PipelineLog>> {
		public Guid PipelineId { get; set; }

		public int PageSize { get; set; }

		public int PageIndex { get; set; }

		public GetAllPipelineLogCommand(Guid pipelineId, int pageSize, int pageIndex) {
			PipelineId = pipelineId;
			PageSize = pageSize;
			PageIndex = pageIndex;
		}
	}
}
