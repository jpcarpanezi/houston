namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.GetAll {
	public sealed record GetAllConnectorCommand(int PageSize, int PageIndex) : IRequest<IResultCommand>;
}
