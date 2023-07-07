using Houston.Core.Entities.Postgres;
using Houston.Core.Enums;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class CreateUserCommand : IRequest<ResultCommand<User>> {
		public string Name { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string TempPassword { get; set; } = null!;

		public UserRoleEnum UserRole { get; set; }

		public CreateUserCommand() { }

		public CreateUserCommand(string name, string email, string tempPassword, UserRoleEnum userRole) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Email = email ?? throw new ArgumentNullException(nameof(email));
			TempPassword = tempPassword ?? throw new ArgumentNullException(nameof(tempPassword));
			UserRole = userRole;
		}
	}
}
