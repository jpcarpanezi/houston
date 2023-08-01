namespace Houston.Application.CommandHandlers.UserCommandHandlers.UpdatePassword {
	public sealed record UpdatePasswordCommand(Guid? UserId, string? OldPassword, string NewPassword) : IRequest<IResultCommand>;
}
