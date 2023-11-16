namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.GetAll {
	public class GetAllConnectorFunctionCommandHandler : IRequestHandler<GetAllConnectorFunctionCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllConnectorFunctionCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllConnectorFunctionCommand request, CancellationToken cancellationToken) {
			var connectorFunctions = await _unitOfWork.ConnectorFunctionRepository.GetAllActivesByConnectorId(request.ConnectorId, request.PageSize, request.PageIndex);
			var count = await _unitOfWork.ConnectorFunctionRepository.CountActivesByConnectorId(request.ConnectorId);
            
			var config = new MapperConfiguration(cfg => cfg.AddProfile<MapProfile>());
			var mapper = new Mapper(config);

			var connectorFunctionSummaryViewModel = connectorFunctions.Select(x => mapper.Map<ConnectorFunctionSummaryViewModel>(x));
			
			var connectorFunctionGroupedViewModel = connectorFunctions.OrderBy(x => x.Name)
																	  .ThenByDescending(x => x.LastUpdate)
																	  .GroupBy(x => new { x.Name, x.ConnectorId })
																	  .Select(x => x.First())
																	  .Select(group => new ConnectorFunctionGroupedViewModel {
																		  Name = group.Name,
																		  Connector = group.Connector.Name,
																		  Versions = connectorFunctionSummaryViewModel.Where(x => x.Name == group.Name).OrderByDescending(x => x.Version).ToList(),
																		  CreatedBy = group.CreatedByNavigation.Name,
																		  CreationDate = group.CreationDate,
																		  UpdatedBy = group.UpdatedByNavigation.Name,
																		  LastUpdate = group.LastUpdate
																	  })
																	  .ToList();
			
			return ResultCommand.Paginated<ConnectorFunctionGroupedViewModel, ConnectorFunctionGroupedViewModel>(connectorFunctionGroupedViewModel, request.PageSize, request.PageIndex, count);
		}
	}
}
