using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class UpdatePasswordCommand : IRequest<ResultCommand<User>> {
		public Guid? UserId { get; set; }

		public string? OldPassword { get; set; }

		public string NewPassword { get; set; } = null!;

		public UpdatePasswordCommand(Guid? userId, string? oldPassword, string newPassword) {
			UserId = userId;
			OldPassword = oldPassword;
			NewPassword = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
		}
	}
}
