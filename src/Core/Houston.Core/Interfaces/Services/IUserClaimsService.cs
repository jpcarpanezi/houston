namespace Houston.Core.Interfaces.Services {
	public interface IUserClaimsService {
		Guid Id { get; }
		string Name { get; }
		string Email { get; }
		List<UserRole> Roles { get; }

		Guid GetUserId();
		string GetUserEmail();
		string GetUserName();
		List<UserRole> GetUserRoles(); 
	}
}
