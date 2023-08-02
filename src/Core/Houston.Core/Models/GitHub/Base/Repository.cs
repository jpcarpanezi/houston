namespace Houston.Core.Models.GitHub.Base {
	public class Repository {
		[JsonPropertyName("id")]
		public int Id { get; }

		[JsonPropertyName("ssh_url")]
		public string SshUrl { get; }

		[JsonConstructor]
		public Repository(int id, string sshUrl) {
			Id = id;
			SshUrl = sshUrl ?? throw new ArgumentNullException(nameof(sshUrl));
		}
	}
}
