namespace Houston.Core.Models.GitHub.Push {
	public class PushPayload : GithubPayloadBase {
		[JsonPropertyName("ref")]
		public string Ref { get; }

		[JsonPropertyName("commits")]
		public List<Commit> Commits { get; }

		[JsonPropertyName("head_commit")]
		public HeadCommit HeadCommit { get; }

		[JsonConstructor]
		public PushPayload(Repository repository, Sender sender, string @ref, HeadCommit headCommit, List<Commit> commits) : base(repository, sender) {
			Ref = @ref;
			HeadCommit = headCommit;
			Commits = commits;
		}
	}
}
