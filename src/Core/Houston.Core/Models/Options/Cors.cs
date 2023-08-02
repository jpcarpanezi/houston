namespace Houston.Core.Models.Options {
	public sealed record Cors {
		public string[] AllowedOrigins { get; init; } = null!;

		public string[] AllowedMethods { get; init; } = null!;
	}
}
