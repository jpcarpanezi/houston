namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public sealed record UpdateConnectorFunctionCommand(Guid Id, IFormFile SpecFile, IFormFile Script, IFormFile Package): IRequest<IResultCommand>;
}
