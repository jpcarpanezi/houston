using System.Text.Json.Serialization;

namespace Houston.Core.Models.GitHub.Push {
	public class Author {
		[JsonPropertyName("name")]
		public string Name { get; }

		[JsonPropertyName("email")]
		public string? Email { get; }

		[JsonPropertyName("username")]
		public string Username { get; }

		[JsonConstructor]
		public Author(string name, string? email, string username) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Email = email;
			Username = username ?? throw new ArgumentNullException(nameof(username));
		}
	}
}
