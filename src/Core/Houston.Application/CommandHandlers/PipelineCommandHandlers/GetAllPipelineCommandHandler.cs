using Houston.Core.Commands;
using Houston.Core.Commands.PipelineCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;

namespace Houston.Application.CommandHandlers.PipelineCommandHandlers {
	public class GetAllPipelineCommandHandler : IRequestHandler<GetAllPipelineCommand, PaginatedResultCommand<Pipeline>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllPipelineCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<PaginatedResultCommand<Pipeline>> Handle(GetAllPipelineCommand request, CancellationToken cancellationToken) {
			var connectorFunctions = await _unitOfWork.PipelineRepository.GetAllActives(request.PageSize, request.PageIndex);
			var count = await _unitOfWork.PipelineRepository.CountActives();

			return new PaginatedResultCommand<Pipeline>(connectorFunctions, request.PageSize, request.PageIndex, count);
		}
	}
}
