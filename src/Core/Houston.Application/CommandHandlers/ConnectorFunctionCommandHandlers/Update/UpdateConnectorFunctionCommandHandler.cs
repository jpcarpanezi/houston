namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public class UpdateConnectorFunctionCommandHandler : IRequestHandler<UpdateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;
		private readonly IPublishEndpoint _eventBus;
		private readonly IValidator<ConnectorFunctionSpec> _validator;

		public UpdateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims, IPublishEndpoint eventBus, IValidator<ConnectorFunctionSpec> validator) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		public async Task<IResultCommand> Handle(UpdateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var spec = await ConvertFileToString(request.SpecFile);
			var specsFile = await ConvertFileToByteArray(request.SpecFile);
			var script = await ConvertFileToByteArray(request.Script);
			var package = await ConvertFileToByteArray(request.Package);

			var connectorFunction = await _unitOfWork.ConnectorFunctionRepository.GetActive(request.Id);
			if (connectorFunction is null) {
				return ResultCommand.NotFound("The requested connector function could not be found.", "connectorFunctionNotFound");
			}
			
			var serializer = new DeserializerBuilder()
							 .WithNamingConvention(UnderscoredNamingConvention.Instance)
							 .Build();
			
			var yaml = serializer.Deserialize<ConnectorFunctionSpec>(spec.Trim());

			await _validator.ValidateAndThrowAsync(yaml, cancellationToken);
			
			
			if (SpecFileHasConflict(connectorFunction, yaml, out var conflict))
				return ResultCommand.Conflict(conflict.Item1, conflict.Item2);

			var buildScript = !StructuralComparisons.StructuralEqualityComparer.Equals(request.Script, connectorFunction.Script) || 
							  !StructuralComparisons.StructuralEqualityComparer.Equals(request.Package, connectorFunction.Package);

			connectorFunction.FriendlyName = yaml.FriendlyName;
			connectorFunction.Description = yaml.Description;
			connectorFunction.SpecsFile = specsFile;
			connectorFunction.Script = script;
			connectorFunction.Package = package;
			connectorFunction.BuildStatus = buildScript ? BuildStatus.Unknown : connectorFunction.BuildStatus;
			connectorFunction.UpdatedBy = _claims.Id;
			connectorFunction.LastUpdate = DateTime.UtcNow;

			_unitOfWork.ConnectorFunctionRepository.Update(connectorFunction);
			await _unitOfWork.Commit();
			
			if (buildScript) {
				await _eventBus.Publish(new BuildConnectorFunctionMessage(connectorFunction.Id), cancellationToken);
			}
			
			return ResultCommand.Ok<ConnectorFunction, ConnectorFunctionDetailViewModel>(connectorFunction);
		}

		private static bool SpecFileHasConflict(ConnectorFunction connectorFunction, ConnectorFunctionSpec yaml, out (string, string) conflict) {
			if (connectorFunction.Name != yaml.Function) {
				conflict = ("The name of the connector function cannot be changed.", "connectorFunctionNameCannotBeChanged");
				return true;
			}

			if (connectorFunction.Version != yaml.Version) {
				conflict = ("The version of the connector function cannot be changed.", "connectorFunctionVersionCannotBeChanged");
				return true;
			}

			if (connectorFunction.Connector.Name != yaml.Connector) {
				conflict = ("The connector of the connector function cannot be changed.", "connectorFunctionConnectorCannotBeChanged");
				return true;
			}

			conflict = (string.Empty, string.Empty);
			return false;
		}

		private static async Task<byte[]> ConvertFileToByteArray(IFormFile file) {
			using var memoryStream = new MemoryStream();
			await file.CopyToAsync(memoryStream);
			return memoryStream.ToArray();
		}
		
		private static async Task<string> ConvertFileToString(IFormFile file) {
			using var streamReader = new StreamReader(file.OpenReadStream());
			return await streamReader.ReadToEndAsync();
		}
	}
}
