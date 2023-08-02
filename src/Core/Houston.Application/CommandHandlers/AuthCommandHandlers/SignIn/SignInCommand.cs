namespace Houston.Application.CommandHandlers.AuthCommandHandlers.SignIn {
	public sealed record SignInCommand(string Email, string Password) : IRequest<IResultCommand>;
}
