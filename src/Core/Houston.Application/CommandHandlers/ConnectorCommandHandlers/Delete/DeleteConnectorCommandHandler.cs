namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Delete {
	public class DeleteConnectorCommandHandler : IRequestHandler<DeleteConnectorCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public DeleteConnectorCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(DeleteConnectorCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetByIdAsync(request.ConnectorId);
			if (connector is null || !connector.Active) {
				return ResultCommand.NotFound("The requested connector could not be found.", "connectorNotFound");
			}

			connector.Active = false;
			connector.UpdatedBy = _claims.Id;
			connector.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorRepository.Update(connector);
			await _unitOfWork.Commit();

			return ResultCommand.NoContent();
		}
	}
}
