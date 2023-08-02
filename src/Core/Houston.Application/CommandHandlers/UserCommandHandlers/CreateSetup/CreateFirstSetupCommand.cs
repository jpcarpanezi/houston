namespace Houston.Application.CommandHandlers.UserCommandHandlers.CreateSetup {
	public sealed record CreateFirstSetupCommand(string RegistryAddress, string RegistryEmail, string RegistryUsername, string RegistryPassword, string UserName, string UserEmail, string UserPassword) : IRequest<IResultCommand>;
}
