using Houston.Core.Models;
using System.Security.Cryptography;

namespace Houston.Infrastructure.Services {
	public static class DeployKeysService {
		public static DeployKeys Create(int keySize = 4096) {
			using var rsa = RSA.Create();
			rsa.KeySize = keySize;

			var privateKeyBytes = rsa.ExportRSAPrivateKey();
			var privateKeyString = Convert.ToBase64String(privateKeyBytes);

			var publicKeyBytes = rsa.ExportRSAPublicKey();
			var publicKeyString = Convert.ToBase64String(publicKeyBytes);

			return new DeployKeys(privateKeyString, publicKeyString);
		}
	}
}
