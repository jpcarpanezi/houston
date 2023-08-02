namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Get {
	public sealed record GetConnectorCommand(Guid ConnectorId) : IRequest<IResultCommand>;
}
