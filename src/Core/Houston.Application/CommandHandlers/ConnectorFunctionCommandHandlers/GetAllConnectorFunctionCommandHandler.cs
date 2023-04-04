using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;

namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers {
	public class GetAllConnectorFunctionCommandHandler : IRequestHandler<GetAllConnectorFunctionCommand, PaginatedResultCommand<ConnectorFunction>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllConnectorFunctionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<PaginatedResultCommand<ConnectorFunction>> Handle(GetAllConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunctions = await _unitOfWork.ConnectorFunctionRepository.GetAllActivesByConnectorId(request.ConnectorId, request.PageSize, request.PageIndex);
			var count = await _unitOfWork.ConnectorFunctionRepository.CountActivesByConnectorId(request.ConnectorId);

			return new PaginatedResultCommand<ConnectorFunction>(connectorFunctions, request.PageSize, request.PageIndex, count);
		}
	}
}
