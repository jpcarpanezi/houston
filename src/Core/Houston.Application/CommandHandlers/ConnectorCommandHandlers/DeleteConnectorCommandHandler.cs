using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers {
	public class DeleteConnectorCommandHandler : IRequestHandler<DeleteConnectorCommand, ResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public DeleteConnectorCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand> Handle(DeleteConnectorCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetByIdAsync(request.ConnectorId);
			if (connector is null) {
				return new ResultCommand(HttpStatusCode.NotFound, "The requested connector could not be found.");
			}

			if (!connector.Active) {
				return new ResultCommand(HttpStatusCode.NotFound, "The requested pipeline could not be found.");
			}

			connector.Active = false;
			connector.UpdatedBy = _claims.Id;
			connector.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorRepository.Update(connector);
			await _unitOfWork.Commit();

			return new ResultCommand(HttpStatusCode.NoContent, null);
		}
	}
}
