using Houston.Core.Enums;

namespace Houston.Core.Interfaces.Services {
	public interface IUserClaimsService {
		Guid Id { get; }
		string Name { get; }
		string Email { get; }
		List<UserRoleEnum> Roles { get; }

		Guid GetUserId();
		string GetUserEmail();
		string GetUserName();
		List<UserRoleEnum> GetUserRoles(); 
	}
}
