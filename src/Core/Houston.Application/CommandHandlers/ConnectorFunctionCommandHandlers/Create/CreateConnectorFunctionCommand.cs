namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public sealed record CreateConnectorFunctionCommand(string Name, string? Description, Guid ConnectorId, List<CreateConnectorFunctionInputCommand>? Inputs, byte[] Script, byte[] Package, string Version) : IRequest<IResultCommand>;
}
