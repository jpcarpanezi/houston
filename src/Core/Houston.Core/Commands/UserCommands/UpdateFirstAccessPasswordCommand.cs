using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class UpdateFirstAccessPasswordCommand : IRequest<ResultCommand<User>> {
		public string Email { get; set; } = null!;

		public string Token { get; set; } = null!;

		public string Password { get; set; } = null!;

		public UpdateFirstAccessPasswordCommand(string email, string token, string password) {
			Email = email ?? throw new ArgumentNullException(nameof(email));
			Token = token ?? throw new ArgumentNullException(nameof(token));
			Password = password ?? throw new ArgumentNullException(nameof(password));
		}
	}
}
