using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers {
	public class UpdateConnectorCommandHandler : IRequestHandler<UpdateConnectorCommand, ResultCommand<Connector>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public UpdateConnectorCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<Connector>> Handle(UpdateConnectorCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetByIdWithInverseProperties(request.ConnectorId);
			if (connector is null) {
				return new ResultCommand<Connector>(HttpStatusCode.Forbidden, "invalidConnector", null);
			}

			if (!connector.Active) {
				return new ResultCommand<Connector>(HttpStatusCode.Forbidden, "invalidConnector", null);
			}

			connector.Name = request.Name;
			connector.Description = request.Description;
			connector.UpdatedBy = _claims.Id;
			connector.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorRepository.Update(connector);
			await _unitOfWork.Commit();

			return new ResultCommand<Connector>(HttpStatusCode.OK, null, connector);
		}
	}
}
