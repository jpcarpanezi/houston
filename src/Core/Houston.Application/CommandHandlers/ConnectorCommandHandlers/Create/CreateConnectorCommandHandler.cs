namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Create {
	public class CreateConnectorCommandHandler : IRequestHandler<CreateConnectorCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreateConnectorCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreateConnectorCommand request, CancellationToken cancellationToken) {
			var connectorId = Guid.NewGuid();
			Connector connector = new() {
				Id = connectorId,
				Name = request.Name,
				Description = request.Description,
				Active = true,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorRepository.Add(connector);
			await _unitOfWork.Commit();

			var response = await _unitOfWork.ConnectorRepository.GetByIdWithInverseProperties(connectorId);

			return ResultCommand.Created<Connector, ConnectorViewModel>(response!);
		}
	}
}
