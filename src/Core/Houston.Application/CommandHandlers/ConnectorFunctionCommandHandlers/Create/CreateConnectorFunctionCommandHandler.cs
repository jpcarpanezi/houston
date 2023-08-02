namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class CreateConnectorFunctionCommandHandler : IRequestHandler<CreateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<IResultCommand> Handle(CreateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunctionInputs = new List<ConnectorFunctionInput>();
			var connectorFunctionId = Guid.NewGuid();

			if (request.Inputs is not null) {
				foreach (var input in request.Inputs) {
					var connectorFunctionInput = new ConnectorFunctionInput {
						Id = Guid.NewGuid(),
						ConnectorFunctionId = connectorFunctionId,
						Name = input.Name,
						Placeholder = input.Placeholder,
						Type = input.InputType,
						Required = input.Required,
						Replace = input.Replace,
						Values = input.Values,
						DefaultValue = input.DefaultValue,
						AdvancedOption = input.AdvancedOption,
						CreatedBy = _claims.Id,
						CreationDate = DateTime.UtcNow,
						UpdatedBy = _claims.Id,
						LastUpdate = DateTime.UtcNow
					};

					connectorFunctionInputs.Add(connectorFunctionInput);
				}
			}

			var connectorFunction = new ConnectorFunction {
				Id = connectorFunctionId,
				Name = request.Name,
				Description = request.Description,
				Active = true,
				ConnectorId = request.ConnectorId,
				Script = request.Script,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorFunctionRepository.Add(connectorFunction);
			_unitOfWork.ConnectorFunctionInputRepository.AddRange(connectorFunctionInputs);

			await _unitOfWork.Commit();

			return ResultCommand.Created<ConnectorFunction, ConnectorFunctionViewModel>(connectorFunction);
		}
	}
}
