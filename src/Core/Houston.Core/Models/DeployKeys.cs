namespace Houston.Core.Models {
	public class DeployKeys {
		public string PrivateKey { get; set; } = null!;

		public string PublicKey { get; set; } = null!;

		public DeployKeys(string privateKey, string publicKey) {
			PrivateKey = privateKey ?? throw new ArgumentNullException(nameof(privateKey));
			PublicKey = publicKey ?? throw new ArgumentNullException(nameof(publicKey));
		}
	}
}
