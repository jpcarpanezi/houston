using Houston.Core.Entities.MongoDB;
using MediatR;
using MongoDB.Bson;

namespace Houston.Core.Commands.UserCommands {
	public class ToggleUserStatusCommand : IRequest<ResultCommand<User>> {
		public ObjectId UserId { get; set; }

		public ToggleUserStatusCommand(ObjectId userId) {
			UserId = userId;
		}
	}
}
