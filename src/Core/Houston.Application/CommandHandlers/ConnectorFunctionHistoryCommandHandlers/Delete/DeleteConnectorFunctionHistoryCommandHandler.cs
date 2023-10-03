namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Delete {
	public class DeleteConnectorFunctionHistoryCommandHandler : IRequestHandler<DeleteConnectorFunctionHistoryCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public DeleteConnectorFunctionHistoryCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(DeleteConnectorFunctionHistoryCommand request, CancellationToken cancellationToken) {
			var connectorFunctionHistory = await _unitOfWork.ConnectorFunctionHistoryRepository.GetActive(request.Id);
			if (connectorFunctionHistory is null) {
				return ResultCommand.NotFound("The requested connector function history could not be found.", "connectorFunctionHistoryNotFound");
			}

			connectorFunctionHistory.Active = false;
			connectorFunctionHistory.UpdatedBy = _claims.Id;
			connectorFunctionHistory.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorFunctionHistoryRepository.Update(connectorFunctionHistory);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
