namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Update {
	public class UpdateConnectorCommandHandler : IRequestHandler<UpdateConnectorCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdateConnectorCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdateConnectorCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetByIdWithInverseProperties(request.ConnectorId);
			if (connector is null || !connector.Active) {
				return ResultCommand.NotFound("The requested connector could not be found.", "connectorNotFound");
			}

			connector.Name = request.Name;
			connector.Description = request.Description;
			connector.UpdatedBy = _claims.Id;
			connector.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorRepository.Update(connector);
			await _unitOfWork.Commit();

			return ResultCommand.Ok<Connector, ConnectorViewModel>(connector);
		}
	}
}
