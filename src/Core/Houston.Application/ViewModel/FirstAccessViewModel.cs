namespace Houston.Application.ViewModel {
	public class FirstAccessViewModel : MessageViewModel {
		public string PasswordToken { get; set; }

		public string Location { get; set; }

		public FirstAccessViewModel(string passwordToken, string location, string message) : base(message) {
			PasswordToken = passwordToken;
			Location = location;
		}
	}
}
