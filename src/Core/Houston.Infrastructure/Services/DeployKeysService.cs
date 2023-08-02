namespace Houston.Infrastructure.Services {
	public static class DeployKeysService {
		public static DeployKeys Create(string comment, int keySize = 2048) {
			var keygen = new SshKeyGenerator.SshKeyGenerator(keySize);

			string privateKey = keygen.ToPrivateKey();
			string publicKey = keygen.ToRfcPublicKey(comment);

			return new DeployKeys(privateKey, publicKey);
		}
	}
}
