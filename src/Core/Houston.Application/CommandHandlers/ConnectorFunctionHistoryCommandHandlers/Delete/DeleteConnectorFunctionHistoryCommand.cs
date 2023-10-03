namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Delete {
	public sealed record DeleteConnectorFunctionHistoryCommand(Guid Id) : IRequest<IResultCommand>;
}
