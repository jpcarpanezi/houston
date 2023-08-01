namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public sealed record UpdateConnectorFunctionInputCommand(Guid? Id, InputTypeEnum InputType, string Name, string Placeholder, string Replace, bool Required, string? DefaultValue, string[]? Values, bool AdvancedOption);
}
