using System.Collections;

namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Update {
	public class UpdateConnectorFunctionHistoryCommandHandler : IRequestHandler<UpdateConnectorFunctionHistoryCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;
		private readonly IPublishEndpoint _eventBus;

		public UpdateConnectorFunctionHistoryCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims, IPublishEndpoint eventBus) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
		}

		public async Task<IResultCommand> Handle(UpdateConnectorFunctionHistoryCommand request, CancellationToken cancellationToken) {
			var connectorFunctionHistory = await _unitOfWork.ConnectorFunctionHistoryRepository.GetByIdWithInputs(request.Id);
			if (connectorFunctionHistory is null) {
				return ResultCommand.NotFound("The requested connector function could not be found.", "connectorFunctionNotFound");
			}

			var buildScript = false;
			if (!StructuralComparisons.StructuralEqualityComparer.Equals(request.Script, connectorFunctionHistory.Script) ||
				!StructuralComparisons.StructuralEqualityComparer.Equals(request.Package, connectorFunctionHistory.Package)) {
				buildScript = true;
			}

			connectorFunctionHistory.Script = request.Script;
			connectorFunctionHistory.Package = request.Package;
			connectorFunctionHistory.BuildStatus = BuildStatus.Unknown;
			connectorFunctionHistory.UpdatedBy = _claims.Id;
			connectorFunctionHistory.LastUpdate = DateTime.UtcNow;

			foreach (var input in connectorFunctionHistory.ConnectorFunctionInputs) {
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

					connectorFunctionHistory.ConnectorFunctionInputs.Add(input);
				}
			}

			_unitOfWork.ConnectorFunctionHistoryRepository.Update(connectorFunctionHistory);
			await _unitOfWork.Commit();

			if (buildScript) {
				await _eventBus.Publish(new BuildConnectorFunctionMessage(connectorFunctionHistory.Id), cancellationToken);
			}

			return ResultCommand.Ok<ConnectorFunctionHistory, ConnectorFunctionHistoryDetailViewModel>(connectorFunctionHistory);
		}
	}
}
