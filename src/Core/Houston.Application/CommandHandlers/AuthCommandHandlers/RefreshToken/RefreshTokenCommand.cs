namespace Houston.Application.CommandHandlers.AuthCommandHandlers.RefreshToken {
	public sealed record RefreshTokenCommand(string Token) : IRequest<IResultCommand>;
}
