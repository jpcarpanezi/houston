namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Create {
	public sealed record CreateConnectorCommand(string FriendlyName, string Name, string? Description) : IRequest<IResultCommand>;
}
