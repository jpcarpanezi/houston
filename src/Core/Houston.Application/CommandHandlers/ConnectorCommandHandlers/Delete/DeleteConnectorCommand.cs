namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Delete {
	public sealed record DeleteConnectorCommand(Guid ConnectorId) : IRequest<IResultCommand>;
}
