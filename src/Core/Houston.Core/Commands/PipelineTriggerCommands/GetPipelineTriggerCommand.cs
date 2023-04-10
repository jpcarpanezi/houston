using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineTriggerCommands {
	public class GetPipelineTriggerCommand : IRequest<ResultCommand<PipelineTrigger>> {
		public Guid Id { get; set; }

		public GetPipelineTriggerCommand(Guid id) {
			Id = id;
		}
	}
}
