﻿namespace Houston.Application.CommandHandlers.UserCommandHandlers.Create {
	public sealed record CreateUserCommand(string Name, string Email, string TempPassword, UserRole UserRole) : IRequest<IResultCommand>;
}
