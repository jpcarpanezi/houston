namespace Houston.Application.CommandHandlers.ConnectorFunctionCommandHandlers.Update {
	public sealed record UpdateConnectorFunctionFilesCommand(IFormFile SpecFile, IFormFile Script, IFormFile Package);
}
