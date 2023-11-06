namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Get {
	public class GetConnectorFunctionHistoryCommandHandler : IRequestHandler<GetConnectorFunctionHistoryCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public GetConnectorFunctionHistoryCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(GetConnectorFunctionHistoryCommand request, CancellationToken cancellationToken) {
			var connectorFunctionHistory = await _unitOfWork.ConnectorFunctionHistoryRepository.GetActive(request.Id);
			if (connectorFunctionHistory is null) {
				return ResultCommand.NotFound("The requested connector function history could not be found.", "connectorFunctionHistoryNotFound");
			}

			return ResultCommand.Ok<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>(connectorFunctionHistory);
		}
	}
}
