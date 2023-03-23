using Houston.Core.Enums;

namespace Houston.Application.ViewModel.UserViewModels {
	public class FirstUserViewModel {
		public string Email { get; set; } = null!;

		public string Name { get; set; } = null!;

		public UserRoleEnum UserRole { get; set; }
	}
}
