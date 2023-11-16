namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Create {
	public sealed record CreateConnectorFunctionCommand(IFormFile SpecFile, IFormFile Script, IFormFile Package) : IRequest<IResultCommand>;
}
