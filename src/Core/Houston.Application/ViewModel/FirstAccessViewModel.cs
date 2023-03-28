namespace Houston.Application.ViewModel {
	public class FirstAccessViewModel : MessageViewModel {
		public string PasswordToken { get; set; }

		public FirstAccessViewModel(string passwordToken, string message) : base(message) {
			PasswordToken = passwordToken;
		}
	}
}
