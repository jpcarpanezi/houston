namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public class UpdateConnectorFunctionCommandHandler : IRequestHandler<UpdateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(UpdateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetActive(request.Id);
			if (connectorFunction is null) {
				return ResultCommand.NotFound("The requested connector function could not be found.", "connectorFunctionNotFound");
			}

			connectorFunction.Name = request.Name;
			connectorFunction.Description = request.Description;
			connectorFunction.UpdatedBy = _claims.Id;
			connectorFunction.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorFunctionRepository.Update(connectorFunction);
			await _unitOfWork.Commit();

			return ResultCommand.Ok<ConnectorFunction, ConnectorFunctionViewModel>(connectorFunction);
		}
	}
}
