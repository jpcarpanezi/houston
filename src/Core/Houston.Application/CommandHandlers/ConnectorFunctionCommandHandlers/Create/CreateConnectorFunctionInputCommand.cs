namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public sealed record CreateConnectorFunctionInputCommand(InputType InputType, string Name, string Placeholder, string Replace, bool Required, string? DefaultValue, string[]? Values, bool AdvancedOption);
}
