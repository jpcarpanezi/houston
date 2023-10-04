namespace Houston.Application.CommandHandlers.ConnectorFunctionHistoryCommandHandlers.Update {
	public sealed record UpdateConnectorFunctionHistoryCommand(Guid Id, byte[] Script, byte[] Package, List<UpdateConnectorFunctionInputCommand> Inputs) : IRequest<IResultCommand>;

	public sealed record UpdateConnectorFunctionInputCommand(Guid? Id, InputType InputType, string Name, string Placeholder, string Replace, bool Required, string? DefaultValue, string[]? Values, bool AdvancedOption);
}
