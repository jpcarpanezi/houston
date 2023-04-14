using Houston.Core.Commands;
using Houston.Core.Commands.PipelineTriggerCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineTriggerCommandHandlers {
	public class GetPipelineTriggerCommandHandler : IRequestHandler<GetPipelineTriggerCommand, ResultCommand<PipelineTrigger>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetPipelineTriggerCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<PipelineTrigger>> Handle(GetPipelineTriggerCommand request, CancellationToken cancellationToken) {
			var pipelineTrigger = await _unitOfWork.PipelineTriggerRepository.GetByIdWithInverseProperties(request.Id);
			if (pipelineTrigger is null) {
				return new ResultCommand<PipelineTrigger>(HttpStatusCode.NotFound, "The requested pipeline trigger could not be found.", "pipelineTriggerNotFound", null);
			}

			return new ResultCommand<PipelineTrigger>(HttpStatusCode.OK, null, null, pipelineTrigger);
		}
	}
}
