namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public class CreateConnectorFunctionCommandHandler : IRequestHandler<CreateConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserClaimsService _claims;
		private readonly IPublishEndpoint _eventBus;
		private readonly IValidator<ConnectorFunctionSpec> _validator;

		public CreateConnectorFunctionCommandHandler(IUnitOfWork unitOfWork, IUserClaimsService claims, IPublishEndpoint eventBus, IValidator<ConnectorFunctionSpec> validator) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
			_claims = claims ?? throw new ArgumentNullException(nameof(claims));
			_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
			_validator = validator ?? throw new ArgumentNullException(nameof(validator));
		}

		public async Task<IResultCommand> Handle(CreateConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var spec = await ConvertFileToString(request.SpecFile);
			var specsFile = await ConvertFileToByteArray(request.SpecFile);
			var script = await ConvertFileToByteArray(request.Script);
			var package = await ConvertFileToByteArray(request.Package); 
			
			var serializer = new DeserializerBuilder()
							 .WithNamingConvention(UnderscoredNamingConvention.Instance)
							 .Build();
			
			var yaml = serializer.Deserialize<ConnectorFunctionSpec>(spec.Trim());

			await _validator.ValidateAndThrowAsync(yaml, cancellationToken);

			var connector = await _unitOfWork.ConnectorRepository.GetActiveByName(yaml.Connector);
			if (connector is null) {
				return ResultCommand.NotFound("The requested connector could not be found.", "connectorNotFound");
			}

			var anyConnectorFunctionActive = await _unitOfWork.ConnectorFunctionRepository.AnyActive(connector.Id, yaml.Function, yaml.Version);
			if (anyConnectorFunctionActive) {
				return ResultCommand.Conflict("There is already a connector function with this name and version.", "connectorFunctionAlreadyExists");
			}
			
			var connectorFunction = new ConnectorFunction {
				Id = Guid.NewGuid(),
				Name = yaml.Function,
				FriendlyName = yaml.FriendlyName,
				Description = yaml.Description,
				ConnectorId = connector.Id,
				Active = true,
				Version = yaml.Version,
				SpecsFile = specsFile,
				Script = script,
				Package = package,
				BuildStatus = BuildStatus.Unknown,
				CreatedBy = _claims.Id,
				CreationDate = DateTime.UtcNow,
				UpdatedBy = _claims.Id,
				LastUpdate = DateTime.UtcNow
			};

			_unitOfWork.ConnectorFunctionRepository.Add(connectorFunction);
			await _unitOfWork.Commit();

			await _eventBus.Publish(new BuildConnectorFunctionMessage(connectorFunction.Id), cancellationToken);

			return ResultCommand.Created<ConnectorFunction, ConnectorFunctionDetailViewModel>(connectorFunction);
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
