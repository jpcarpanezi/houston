namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.GetAll {
	public class GetAllConnectorCommandHandler : IRequestHandler<GetAllConnectorCommand, IResultCommand> {
		private readonly IUnitOfWork _unitOfWork;

		public GetAllConnectorCommandHandler(IUnitOfWork unitOfWork) {
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<IResultCommand> Handle(GetAllConnectorCommand request, CancellationToken cancellationToken) {
			var connectors = await _unitOfWork.ConnectorRepository.GetAllActives(request.PageSize, request.PageIndex);
			var count = await _unitOfWork.ConnectorRepository.CountActives();

			return ResultCommand.Paginated<Connector, ConnectorViewModel>(connectors, request.PageSize, request.PageIndex, count);
		}
	}
}
