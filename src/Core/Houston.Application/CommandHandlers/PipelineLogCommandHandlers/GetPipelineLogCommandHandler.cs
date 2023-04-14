using Houston.Core.Commands;
using Houston.Core.Commands.PipelineLogCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineLogCommandHandlers {
	public class GetPipelineLogCommandHandler : IRequestHandler<GetPipelineLogCommand, ResultCommand<PipelineLog>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetPipelineLogCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<PipelineLog>> Handle(GetPipelineLogCommand request, CancellationToken cancellationToken) {
			var pipelineLog = await _unitOfWork.PipelineLogsRepository.GetByIdWithInverseProperties(request.Id);
			if (pipelineLog is null) {
				return new ResultCommand<PipelineLog>(HttpStatusCode.NotFound, "The requested pipeline log could not be found.", "pipelineLogNotFound", null);
			}

			return new ResultCommand<PipelineLog>(HttpStatusCode.OK, null, null, pipelineLog);
		}
	}
}
