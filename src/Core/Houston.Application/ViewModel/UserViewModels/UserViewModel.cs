namespace Houston.Application.ViewModel.UserViewModels {
	public class UserViewModel {
		public Guid Id { get; set; }

		public string Email { get; set; } = null!;

		public string Name { get; set; } = null!;

		public bool Active { get; set; }

		public UserRoleEnum Role { get; set; }
	}
}
