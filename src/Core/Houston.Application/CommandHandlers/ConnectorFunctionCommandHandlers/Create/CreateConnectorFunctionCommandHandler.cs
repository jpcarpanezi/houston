namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class CreateConnectorFunctionCommandHandler : IRequestHandler<CreateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;
		private readonly IPublishEndpoint _eventBus;

		public CreateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims, IPublishEndpoint eventBus) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
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
				Package = request.Package,
				Version = request.Version,
				BuildStatus = BuildStatus.Unknown,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorFunctionRepository.Add(connectorFunction);
			_unitOfWork.ConnectorFunctionInputRepository.AddRange(connectorFunctionInputs);

			await _unitOfWork.Commit();

			await _eventBus.Publish(new BuildConnectorFunctionMessage(connectorFunctionId), cancellationToken);

			return ResultCommand.Created<ConnectorFunction, ConnectorFunctionViewModel>(connectorFunction);
		}
	}
}
