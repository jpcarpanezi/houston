namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Get {
	public sealed record GetConnectorFunctionCommand(Guid Id) : IRequest<IResultCommand>;
}
