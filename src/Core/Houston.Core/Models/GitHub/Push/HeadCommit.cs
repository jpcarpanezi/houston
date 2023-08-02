namespace Houston.Core.Models.GitHub.Push {
	public class HeadCommit {
		[JsonPropertyName("id")]
		public string Id { get; }

		[JsonPropertyName("tree_id")]
		public string TreeId { get; }

		[JsonPropertyName("distinct")]
		public bool Distinct { get; }

		[JsonPropertyName("message")]
		public string Message { get; }

		[JsonPropertyName("timestamp")]
		public DateTime Timestamp { get; }

		[JsonPropertyName("url")]
		public string Url { get; }

		[JsonPropertyName("author")]
		public Author Author { get; }

		[JsonPropertyName("committer")]
		public Committer Committer { get; }

		[JsonPropertyName("added")]
		public List<string> Added { get; } 

		[JsonPropertyName("removed")]
		public List<string> Removed { get; }

		[JsonPropertyName("modified")]
		public List<string> Modified { get; }

		[JsonConstructor]
		public HeadCommit(string id, string treeId, bool distinct, string message, DateTime timestamp, string url, Author author, Committer committer, List<string> added, List<string> removed, List<string> modified) {
			Id = id ?? throw new ArgumentNullException(nameof(id));
			TreeId = treeId ?? throw new ArgumentNullException(nameof(treeId));
			Distinct = distinct;
			Message = message ?? throw new ArgumentNullException(nameof(message));
			Timestamp = timestamp;
			Url = url ?? throw new ArgumentNullException(nameof(url));
			Author = author ?? throw new ArgumentNullException(nameof(author));
			Committer = committer ?? throw new ArgumentNullException(nameof(committer));
			Added = added ?? throw new ArgumentNullException(nameof(added));
			Removed = removed ?? throw new ArgumentNullException(nameof(removed));
			Modified = modified ?? throw new ArgumentNullException(nameof(modified));
		}
	}
}
