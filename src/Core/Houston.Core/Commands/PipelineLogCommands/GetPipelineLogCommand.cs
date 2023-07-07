using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineLogCommands {
	public class GetPipelineLogCommand : IRequest<ResultCommand<PipelineLog>> {
		public Guid Id { get; set; }

		public GetPipelineLogCommand(Guid id) {
			Id = id;
		}
	}
}
