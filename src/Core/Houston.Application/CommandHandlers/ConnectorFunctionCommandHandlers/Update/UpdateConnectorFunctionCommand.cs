namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public sealed record UpdateConnectorFunctionCommand(Guid Id, string Name, string? Description, List<UpdateConnectorFunctionInputCommand>? Inputs, string[] Script): IRequest<IResultCommand>;
}
