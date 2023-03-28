using Houston.Core.Enums;
using MongoDB.Bson;

namespace Houston.Core.Interfaces.Services {
	public interface IUserClaimsService {
		ObjectId Id { get; }
		string Name { get; }
		string Email { get; }
		List<UserRoleEnum> Roles { get; }

		ObjectId GetUserId();
		string GetUserEmail();
		string GetUserName();
		List<UserRoleEnum> GetUserRoles(); 
	}
}
