namespace Houston.Application.Security {
	public class TokenConfigurations {
		public string Audience { get; set; } = null!;

		public string Issuer { get; set; } = null!;

		public int Seconds { get; set; }

		public int FinalExpiration { get; set; }

		public TokenConfigurations() { }

		public TokenConfigurations(string audience, string issuer, int seconds, int finalExpiration) {
			Audience = audience ?? throw new ArgumentNullException(nameof(audience));
			Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
			Seconds = seconds;
			FinalExpiration = finalExpiration;
		}
	}
}
