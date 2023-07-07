using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;

namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers {
	public class GetAllConnectorCommandHandler : IRequestHandler<GetAllConnectorCommand, PaginatedResultCommand<Connector>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllConnectorCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<PaginatedResultCommand<Connector>> Handle(GetAllConnectorCommand request, CancellationToken cancellationToken) {
			var connectors = await _unitOfWork.ConnectorRepository.GetAllActives(request.PageSize, request.PageIndex);
			var count = await _unitOfWork.ConnectorRepository.CountActives();

			return new PaginatedResultCommand<Connector>(connectors, request.PageSize, request.PageIndex, count);
		}
	}
}
