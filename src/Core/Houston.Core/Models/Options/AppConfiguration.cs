namespace Houston.Core.Models.Options {
	public sealed record AppConfiguration {
		public string ConfigurationKey { get; init; } = null!;

		public string RunnerImage { get; init; } = null!;

		public string RunnerTag { get; init; } = null!;
	}
}
