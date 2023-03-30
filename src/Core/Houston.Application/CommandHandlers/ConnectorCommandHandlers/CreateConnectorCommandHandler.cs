using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers {
	public class CreateConnectorCommandHandler : IRequestHandler<CreateConnectorCommand, ResultCommand<Connector>> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public CreateConnectorCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand<Connector>> Handle(CreateConnectorCommand request, CancellationToken cancellationToken) {
			var connectorId = Guid.NewGuid();
			Connector connector = new() {
				Id = connectorId,
				Name = request.Name,
				Description = request.Description,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorRepository.Add(connector);
			await _unitOfWork.Commit();

			var response = await _unitOfWork.ConnectorRepository.GetByIdWithInverseProperties(connectorId);

			return new ResultCommand<Connector>(HttpStatusCode.Created, null, response);
		}
	}
}
