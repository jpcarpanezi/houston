using Houston.Core.Entities.MongoDB;
using MediatR;
using MongoDB.Bson;

namespace Houston.Core.Commands.UserCommands {
	public class UpdatePasswordCommand : IRequest<ResultCommand<User>> {
		public ObjectId? UserId { get; set; }

		public string? OldPassword { get; set; }

		public string NewPassword { get; set; } = null!;

		public UpdatePasswordCommand(ObjectId? userId, string? oldPassword, string newPassword) {
			UserId = userId;
			OldPassword = oldPassword;
			NewPassword = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
		}
	}
}
