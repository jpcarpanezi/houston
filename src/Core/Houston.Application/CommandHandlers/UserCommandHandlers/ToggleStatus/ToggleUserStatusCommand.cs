namespace Houston.Application.CommandHandlers.UserCommandHandlers.ToggleStatus {
	public sealed record ToggleUserStatusCommand(Guid UserId) : IRequest<IResultCommand>;
}
