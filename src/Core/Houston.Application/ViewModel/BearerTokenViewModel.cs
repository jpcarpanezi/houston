namespace Houston.Application.ViewModel {
	public class BearerTokenViewModel : MessageViewModel {
		public DateTime CreatedAt { get; set; }

		public DateTime ExpiresAt { get; set; }

		public string AccessToken { get; set; }

		public string RefreshToken { get; set; }

		public BearerTokenViewModel(string message, DateTime createdAt, DateTime expiresAt, string accessToken, string refreshToken) : base(message) {
			CreatedAt = createdAt;
			ExpiresAt = expiresAt;
			AccessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
			RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
		}
	}
}
