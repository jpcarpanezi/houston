namespace Houston.Application.CommandHandlers.UserCommandHandlers.UpdateFirstAccess {
	public sealed record UpdateFirstAccessPasswordCommand(string Email, string Token, string Password) : IRequest<IResultCommand>;
}
