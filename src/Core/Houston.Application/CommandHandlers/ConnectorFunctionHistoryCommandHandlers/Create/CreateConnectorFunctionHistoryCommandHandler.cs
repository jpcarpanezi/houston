namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Create {
	public class CreateConnectorFunctionHistoryCommandHandler : IRequestHandler<CreateConnectorFunctionHistoryCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;
		private readonly IPublishEndpoint _eventBus;

		public CreateConnectorFunctionHistoryCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims, IPublishEndpoint eventBus) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
		}

		public async Task<IResultCommand> Handle(CreateConnectorFunctionHistoryCommand request, CancellationToken cancellationToken) {
			var connectorFunctionInputs = new List<ConnectorFunctionInput>();
			var connectorFunctionHistoryId = Guid.NewGuid();

			if (request.Inputs is not null) {
				foreach (var input in request.Inputs) {
					var connectorFunctionInput = new ConnectorFunctionInput {
						Id = Guid.NewGuid(),
						ConnectorFunctionHistoryId = connectorFunctionHistoryId,
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

			var connectorFunction = new ConnectorFunctionHistory {
				Id = connectorFunctionHistoryId,
				Active = true,
				ConnectorFunctionId = request.ConnectorFunctionId,
				Script = request.Script,
				Package = request.Package,
				Version = request.Version,
				BuildStatus = BuildStatus.Unknown,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorFunctionHistoryRepository.Add(connectorFunction);
			_unitOfWork.ConnectorFunctionInputRepository.AddRange(connectorFunctionInputs);

			await _unitOfWork.Commit();

			await _eventBus.Publish(new BuildConnectorFunctionMessage(connectorFunctionHistoryId), cancellationToken);

			return ResultCommand.Created<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>(connectorFunction);
		}
	}
}
