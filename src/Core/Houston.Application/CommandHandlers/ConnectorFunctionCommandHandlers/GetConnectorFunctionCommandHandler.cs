using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers {
	public class GetConnectorFunctionCommandHandler : IRequestHandler<GetConnectorFunctionCommand, ResultCommand<ConnectorFunction>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetConnectorFunctionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<ConnectorFunction>> Handle(GetConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetActive(request.Id);

			if (connectorFunction is null) {
				return new ResultCommand<ConnectorFunction>(HttpStatusCode.NotFound, "The requested connector function could not be found.", null);
			}

			return new ResultCommand<ConnectorFunction>(HttpStatusCode.OK, null, connectorFunction);
		}
	}
}
