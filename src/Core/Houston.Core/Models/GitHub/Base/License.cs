namespace Houston.Core.Models.GitHub.Base {
	public class License {
		[JsonPropertyName("key")]
		public string Key { get; }

		[JsonPropertyName("name")]
		public string Name { get; }

		[JsonPropertyName("spdx_id")]
		public string SpdxId { get; }

		[JsonPropertyName("url")]
		public string Url { get; }

		[JsonPropertyName("node_id")]
		public string NodeId { get; }

		[JsonConstructor]
		public License(string key, string name, string spdxId, string url, string nodeId) {
			Key = key ?? throw new ArgumentNullException(nameof(key));
			Name = name ?? throw new ArgumentNullException(nameof(name));
			SpdxId = spdxId ?? throw new ArgumentNullException(nameof(spdxId));
			Url = url ?? throw new ArgumentNullException(nameof(url));
			NodeId = nodeId ?? throw new ArgumentNullException(nameof(nodeId));
		}
	}
}
