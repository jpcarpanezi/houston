namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.GetAll {
	public class GetAllConnectorFunctionCommandHandler : IRequestHandler<GetAllConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllConnectorFunctionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunctions = await _unitOfWork.ConnectorFunctionRepository.GetAllActivesByConnectorId(request.ConnectorId, request.PageSize, request.PageIndex);
			var count = await _unitOfWork.ConnectorFunctionRepository.CountActivesByConnectorId(request.ConnectorId);

			return ResultCommand.Paginated<ConnectorFunction, ConnectorFunctionViewModel>(connectorFunctions, request.PageSize, request.PageIndex, count);
		}
	}
}
