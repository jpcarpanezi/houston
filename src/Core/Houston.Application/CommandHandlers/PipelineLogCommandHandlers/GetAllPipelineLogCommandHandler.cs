using Houston.Core.Commands;
using Houston.Core.Commands.PipelineLogCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;

namespace Houston.Application.CommandHandlers.PipelineLogCommandHandlers {
	public class GetAllPipelineLogCommandHandler : IRequestHandler<GetAllPipelineLogCommand, PaginatedResultCommand<PipelineLog>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllPipelineLogCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<PaginatedResultCommand<PipelineLog>> Handle(GetAllPipelineLogCommand request, CancellationToken cancellationToken) {
			var pipelineLogs = await _unitOfWork.PipelineLogsRepository.GetAllByPipelineId(request.PipelineId, request.PageSize, request.PageIndex);
			var count = await _unitOfWork.PipelineLogsRepository.CountByPipelineId(request.PipelineId);

			return new PaginatedResultCommand<PipelineLog>(pipelineLogs, request.PageSize, request.PageIndex, count);
		}
	}
}
