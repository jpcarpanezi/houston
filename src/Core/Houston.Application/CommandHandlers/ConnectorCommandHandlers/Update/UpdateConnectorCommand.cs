namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Update {
	public sealed record UpdateConnectorCommand(Guid ConnectorId, string FriendlyName, string? Description): IRequest<IResultCommand>;
}
