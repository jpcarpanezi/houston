using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class ToggleUserStatusCommand : IRequest<ResultCommand<User>> {
		public Guid UserId { get; set; }

		public ToggleUserStatusCommand(Guid userId) {
			UserId = userId;
		}
	}
}
