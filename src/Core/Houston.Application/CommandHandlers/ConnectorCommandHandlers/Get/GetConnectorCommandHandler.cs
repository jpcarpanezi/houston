namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Get {
	public class GetConnectorCommandHandler : IRequestHandler<GetConnectorCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetConnectorCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetConnectorCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetActive(request.ConnectorId);

			if (connector is null) {
				return ResultCommand.NotFound("The requested connector could not be found.", "connectorNotFound");
			}

			return ResultCommand.Ok<Connector, ConnectorViewModel>(connector);
		}
	}
}
