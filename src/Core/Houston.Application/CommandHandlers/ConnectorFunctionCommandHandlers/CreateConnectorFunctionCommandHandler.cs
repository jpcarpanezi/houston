using Houston.Core.Commands;
using Houston.Core.Commands.ConnectorFunctionCommands;
using Houston.Core.Entities.Postgres;
using Houston.Core.Interfaces.Repository;
using MediatR;
using System.Net;

namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers {
	public class CreateConnectorFunctionCommandHandler : IRequestHandler<CreateConnectorFunctionCommand, ResultCommand<ConnectorFunction>> {
		private readonly IUnitOfWork _unitOfWork;

		public CreateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<ResultCommand<ConnectorFunction>> Handle(CreateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			//Guid userId = ObjectId.Parse("6406274256017a23e89a7dd6");
			//ConnectorFunction connectorFunction = new() {
			//	Id = ObjectId.GenerateNewId(),
			//	Name = request.Name,
			//	Description = request.Description,
			//	ConnectorId = request.ConnectorId ?? ObjectId.Empty,
			//	Dependencies = request.Dependencies,
			//	Version = request.Version ?? "1.0.0",
			//	IsEntrypoint = false,
			//	Inputs = request.Inputs?.Select(x => new ConnectorFunctionInput {
			//		Id = ObjectId.GenerateNewId(),
			//		Name = x.Name,
			//		InputType = x.InputType,
			//		Required = x.Required,
			//		Placeholder = x.Placeholder,
			//		Replace = x.Replace,
			//		Values = x.Values,
			//		DefaultValue = x.DefaultValue,
			//		AdvancedOption = x.AdvancedOption,
			//		CreatedBy = userId,
			//		CreationDate = DateTime.UtcNow,
			//		UpdatedBy = userId,
			//		LastUpdate = DateTime.UtcNow
			//	}).ToList(),
			//	Script = request.Script,
			//	CreatedBy = userId,
			//	CreationDate = DateTime.UtcNow,
			//	UpdatedBy = userId,
			//	LastUpdate = DateTime.UtcNow
			//};

			//await _unitOfWork.ConnectorFunctionRepository.InsertOneAsync(connectorFunction);

			return new ResultCommand<ConnectorFunction>(HttpStatusCode.Created, null, null);
		}
	}
}
