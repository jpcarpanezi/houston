namespace Houston.Application.ViewModel {
	public class FirstAccessViewModel : MessageViewModel {
		public string PasswordToken { get; set; }

		public string Location { get; set; }

		public FirstAccessViewModel(string passwordToken, string location, string message, string? errorCode) : base(message, errorCode) {
			PasswordToken = passwordToken;
			Location = location;
		}
	}
}
