namespace Houston.Core.Models.GitHub.Base {
	public class GithubPayloadBase {
		[JsonPropertyName("repository")]
		public Repository Repository { get; }

		[JsonPropertyName("sender")]
		public Sender Sender { get; }

		[JsonConstructor]
		public GithubPayloadBase(Repository repository, Sender sender) {
			Repository = repository ?? throw new ArgumentNullException(nameof(repository));
			Sender = sender ?? throw new ArgumentNullException(nameof(sender));
		}
	}
}
