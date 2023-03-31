using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers {
	public class GetConnectorCommandHandler : IRequestHandler<GetConnectorCommand, ResultCommand<Connector>> {
		private readonly IUnitOfWork _unitOfWork;

		public GetConnectorCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<Connector>> Handle(GetConnectorCommand request, CancellationToken cancellationToken) {
			var connector = await _unitOfWork.ConnectorRepository.GetActive(request.ConnectorId);

			if (connector is null) {
				return new ResultCommand<Connector>(HttpStatusCode.NotFound, null, null);
			}

			return new ResultCommand<Connector>(HttpStatusCode.OK, null, connector);
		}
	}
}
