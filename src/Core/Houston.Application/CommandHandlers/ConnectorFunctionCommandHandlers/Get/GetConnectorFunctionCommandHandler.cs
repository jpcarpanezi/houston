namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Get {
	public class GetConnectorFunctionCommandHandler : IRequestHandler<GetConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetConnectorFunctionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetActive(request.Id);

			if (connectorFunction is null) {
				return ResultCommand.NotFound("The requested connector function could not be found.", "connectorFunctionNotFound");
			}

			return ResultCommand.Ok<ConnectorFunction, ConnectorFunctionDetailViewModel>(connectorFunction);
		}
	}
}
