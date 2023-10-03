namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Get {
	public sealed record GetConnectorFunctionHistoryCommand(Guid Id) : IRequest<IResultCommand>;
}
