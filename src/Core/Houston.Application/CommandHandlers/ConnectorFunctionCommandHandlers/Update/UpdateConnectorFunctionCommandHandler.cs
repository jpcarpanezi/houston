namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public class UpdateConnectorFunctionCommandHandler : IRequestHandler<UpdateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;
		private readonly IPublishEndpoint _eventBus;

		public UpdateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims, IPublishEndpoint eventBus) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
		}

		public async Task<IResultCommand> Handle(UpdateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetByIdWithInputs(request.Id);
			if (connectorFunction is null) {
				return ResultCommand.NotFound("The requested connector function could not be found.", "connectorFunctionNotFound");
			}

			var buildScript = false;
			if (request.Script != connectorFunction.Script || request.Package != connectorFunction.Package) {
				buildScript = true;
			}
			
			connectorFunction.Name = request.Name;
			connectorFunction.Description = request.Description;
			connectorFunction.Script = request.Script;
			connectorFunction.Package = request.Package;
			connectorFunction.Version = request.Version;
			connectorFunction.BuildStatus = BuildStatus.Unknown;
			connectorFunction.UpdatedBy = _claims.Id;
			connectorFunction.LastUpdate = DateTime.UtcNow;

			foreach (var input in connectorFunction.ConnectorFunctionInputs) {
				var filterInputUpdate = request.Inputs?.FirstOrDefault(x => x.Id == input.Id);
				if (filterInputUpdate is null) {
					_unitOfWork.ConnectorFunctionInputRepository.Remove(input);
					continue;
				}

				input.Name = filterInputUpdate.Name;
				input.Placeholder = filterInputUpdate.Placeholder;
				input.Type = filterInputUpdate.InputType;
				input.Required = filterInputUpdate.Required;
				input.Replace = filterInputUpdate.Replace;
				input.Values = filterInputUpdate.Values;
				input.DefaultValue = filterInputUpdate.DefaultValue;
				input.AdvancedOption = filterInputUpdate.AdvancedOption;
				input.UpdatedBy = _claims.Id;
				input.LastUpdate = DateTime.UtcNow;

				request.Inputs?.Remove(filterInputUpdate);
			}

			if (request.Inputs?.Count > 0) {
				foreach (var requestInput in request.Inputs) {
					var input = new ConnectorFunctionInput {
						Id = Guid.NewGuid(),
						Name = requestInput.Name,
						Placeholder = requestInput.Placeholder,
						Type = requestInput.InputType,
						Required = requestInput.Required,
						Replace = requestInput.Replace,
						Values = requestInput.Values,
						DefaultValue = requestInput.DefaultValue,
						AdvancedOption = requestInput.AdvancedOption,
						CreatedBy = _claims.Id,
						CreationDate = DateTime.UtcNow,
						UpdatedBy = _claims.Id,
						LastUpdate = DateTime.UtcNow
					};

					connectorFunction.ConnectorFunctionInputs.Add(input);
				}
			}

			_unitOfWork.ConnectorFunctionRepository.Update(connectorFunction);
			await _unitOfWork.Commit();

			if (buildScript) {
				await _eventBus.Publish(new BuildConnectorFunctionMessage(connectorFunction.Id), cancellationToken);
			}

			return ResultCommand.Ok<ConnectorFunction, ConnectorFunctionViewModel>(connectorFunction);
		}
	}
}
