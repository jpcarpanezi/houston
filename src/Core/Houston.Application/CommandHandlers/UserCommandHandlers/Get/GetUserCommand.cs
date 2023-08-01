namespace Houston.Application.CommandHandlers.UserCommandHandlers.Get {
	public sealed record GetUserCommand(Guid Id) : IRequest<IResultCommand>;
}
