namespace Houston.Application.CommandHandlers.ConnectorCommandHandlers.Update {
	public sealed record UpdateConnectorCommand(Guid ConnectorId, string Name, string? Description): IRequest<IResultCommand>;
}
