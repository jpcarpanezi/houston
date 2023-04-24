using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.UserCommands {
	public class GetUserCommand : IRequest<ResultCommand<User>> {
		public Guid Id { get; set; }

		public GetUserCommand() { }

		public GetUserCommand(Guid id) {
			Id = id;
		}
	}
}
