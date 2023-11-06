namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class CreateConnectorFunctionCommandHandler : IRequestHandler<CreateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = new ConnectorFunction {
				Id = Guid.NewGuid(),
				Name = request.Name,
				Description = request.Description,
				ConnectorId = request.ConnectorId,
				Active = true,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorFunctionRepository.Add(connectorFunction);
			await _unitOfWork.Commit();

			return ResultCommand.Created<ConnectorFunction, ConnectorFunctionViewModel>(connectorFunction);
		}
	}
}
