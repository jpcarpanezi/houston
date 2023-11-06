namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Create {
	public sealed record CreateConnectorFunctionHistoryCommand(Guid ConnectorFunctionId, byte[] Script, byte[] Package, string Version, List<CreateConnectorFunctionInputCommand> Inputs) : IRequest<IResultCommand>;

	public sealed record CreateConnectorFunctionInputCommand(InputType InputType, string Name, string Placeholder, string Replace, bool Required, string? DefaultValue, string[]? Values, bool AdvancedOption);
}
