namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.GetAll {
	public sealed record GetAllConnectorFunctionCommand(Guid ConnectorId, int PageSize, int PageIndex) : IRequest<IResultCommand>;
}
