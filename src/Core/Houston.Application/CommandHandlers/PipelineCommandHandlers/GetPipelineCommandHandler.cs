using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class GetPipelineCommandHandler : IRequestHandler<GetPipelineCommand, ResultCommand<Pipeline>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetPipelineCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<Pipeline>> Handle(GetPipelineCommand request, CancellationToken cancellationToken) {
			var pipeline = await _unitOfWork.PipelineRepository.GetActive(request.Id);
			if (pipeline is null) {
				return new ResultCommand<Pipeline>(HttpStatusCode.NotFound, "The requested pipeline could not be found.", "pipelineNotFound", null);
			}

			return new ResultCommand<Pipeline>(HttpStatusCode.OK, null, null, pipeline);
		}
	}
}
