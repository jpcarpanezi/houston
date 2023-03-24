using Houston.Core.Enums;
using Houston.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Security.Claims;

namespace Houston.Infrastructure.Services {
	public class UserClaimsService : IUserClaimsService {
		private readonly IHttpContextAccessor _context;

		public ObjectId Id { get => GetUserId(); }

		public string Name { get => GetUserName(); }

		public string Email { get => GetUserEmail(); }

		public List<UserRoleEnum> Roles { get => GetUserRoles(); }

		public UserClaimsService(IHttpContextAccessor context) {
			_context = context;
		}

		public ObjectId GetUserId() {
			string userGuid = _context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.NameIdentifier), "Cannot get user UUID from JWT.");

			return ObjectId.Parse(userGuid);
		}

		public string GetUserEmail() {
			return _context.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.Email), "Cannot get user e-mail from JWT.");
		}

		public string GetUserName() {
			return _context.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value ?? throw new ArgumentNullException(nameof(ClaimTypes.Name), "Cannot get user name from JWT.");
		}

		public List<UserRoleEnum> GetUserRoles() {
			return _context.HttpContext?.User.FindAll(ClaimTypes.Role).Select(x => (UserRoleEnum) Enum.Parse(typeof(UserRoleEnum), x.Value)).ToList() ?? throw new ArgumentNullException(nameof(ClaimTypes.Role), "Cannot get user roles from JWT.");
		}
	}
}
