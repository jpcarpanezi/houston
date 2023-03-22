namespace Houston.Core.Entities.Redis {
	public class RefreshTokenData {
		public string RefreshToken { get; set; } = null!;

		public string UserId { get; set; } = null!;

		public string UserEmail { get; set; } = null!;

		public RefreshTokenData() { }

		public RefreshTokenData(string refreshToken, string userId, string userEmail) {
			RefreshToken = refreshToken ?? throw new ArgumentNullException(nameof(refreshToken));
			UserId = userId ?? throw new ArgumentNullException(nameof(userId));
			UserEmail = userEmail ?? throw new ArgumentNullException(nameof(userEmail));
		}
	}
}
