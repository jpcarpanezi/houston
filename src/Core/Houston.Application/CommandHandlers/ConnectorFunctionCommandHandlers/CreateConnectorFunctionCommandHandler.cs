using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers {
	public class CreateConnectorFunctionCommandHandler : IRequestHandler<CreateConnectorFunctionCommand, ResultCommand<ConnectorFunction>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<ConnectorFunction>> Handle(CreateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetActive(request.ConnectorId);
			if (connector is null) {
				return new ResultCommand<ConnectorFunction>(HttpStatusCode.Forbidden, "invalidConnector", null);
			}

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
				ConnectorId = request.ConnectorId,
				Script = request.Script,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow,
			};

			_unitOfWork.ConnectorFunctionRepository.Add(connectorFunction);

			if (connectorFunctionInputs.Any())
				_unitOfWork.ConnectorFunctionInputRepository.AddRange(connectorFunctionInputs);

			await _unitOfWork.Commit();

			return new ResultCommand<ConnectorFunction>(HttpStatusCode.Created, null, connectorFunction);
		}
	}
}
