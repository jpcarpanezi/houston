using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class GetPipelineCommand : IRequest<ResultCommand<Pipeline>> {
		public Guid Id { get; set; }

		public GetPipelineCommand() { }

		public GetPipelineCommand(Guid id) {
			Id = id;
		}
	}
}
