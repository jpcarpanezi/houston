using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorCommands;
using Houston.Core.Entities.MongoDB;
using Houston.Core.Interfaces.Repository;
using MediatR;
using MongoDB.Bson;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers {
	public class CreateConnectorCommandHandler : IRequestHandler<CreateConnectorCommand, ResultCommand<Connector>> {
		private readonly IUnitOfWork _unitOfWork;

		public CreateConnectorCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<Connector>> Handle(CreateConnectorCommand request, CancellationToken cancellationToken) {
			Connector connector = new() {
				Name = request.Name,
				Description = request.Description,
				CreatedBy = ObjectId.Parse("6406274256017a23e89a7dd6"),
				CreationDate = DateTime.UtcNow,
				UpdatedBy = ObjectId.Parse("6406274256017a23e89a7dd6"),
				LastUpdate = DateTime.UtcNow
			};

			await _unitOfWork.ConnectorRepository.InsertOneAsync(connector);

			return new ResultCommand<Connector>(HttpStatusCode.Created, "Sucesso", connector);
		}
	}
}
