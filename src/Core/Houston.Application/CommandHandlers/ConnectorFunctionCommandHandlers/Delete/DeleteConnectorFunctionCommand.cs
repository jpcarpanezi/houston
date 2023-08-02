namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Delete {
	public sealed record DeleteConnectorFunctionCommand(Guid Id) : IRequest<IResultCommand>;
}
