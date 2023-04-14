using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers {
	public class UpdateConnectorFunctionCommandHandler : IRequestHandler<UpdateConnectorFunctionCommand, ResultCommand<ConnectorFunction>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<ConnectorFunction>> Handle(UpdateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetByIdWithInputs(request.ConnectorFunctionId);
			if (connectorFunction is null) {
				return new ResultCommand<ConnectorFunction>(HttpStatusCode.NotFound, "The requested connector function could not be found.", "connectorFunctionNotFound", null);
			}

			connectorFunction.Name = request.Name;
			connectorFunction.Description = request.Description;
			connectorFunction.Script = request.Script;
			connectorFunction.UpdatedBy = _claims.Id;
			connectorFunction.LastUpdate = DateTime.UtcNow;

			foreach (var input in connectorFunction.ConnectorFunctionInputs) {
				var filterInputUpdate = request.Inputs?.FirstOrDefault(x => x.ConnectorFunctionInputId == input.Id);
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

			return new ResultCommand<ConnectorFunction>(HttpStatusCode.OK, null, null, connectorFunction);
		}
	}
}
