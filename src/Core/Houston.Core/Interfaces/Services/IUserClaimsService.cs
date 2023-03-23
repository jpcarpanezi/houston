using MongoDB.Bson;

namespace Houston.Core.Interfaces.Services {
	public interface IUserClaimsService {
		ObjectId Id { get; }
		string Name { get; }
		string Email { get; }

		ObjectId GetUserId();
		string GetUserEmail();
		string GetUserName();
	}
}
