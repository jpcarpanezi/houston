using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers {
	public class DeleteConnectorFunctionCommandHandler : IRequestHandler<DeleteConnectorFunctionCommand, ResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;

		public DeleteConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
		}

		public async Task<ResultCommand> Handle(DeleteConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetActive(request.Id);
			if (connectorFunction is null) {
				return new ResultCommand(HttpStatusCode.NotFound, "The requested connector function could not be found.", "connectorFunctionNotFound");
			}

			connectorFunction.Active = false;
			connectorFunction.UpdatedBy = _claims.Id;
			connectorFunction.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorFunctionRepository.Update(connectorFunction);
			await _unitOfWork.Commit();

			return new ResultCommand(HttpStatusCode.NoContent);
		}
	}
}
