using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography;

namespace Houston.API.AuthConfigurations {
	public class SigningConfigurations {
		public SecurityKey Key { get; }

		public SigningCredentials SigningCredentials { get; }

		public SigningConfigurations() {
			using RSACryptoServiceProvider provider = new();
			provider.ImportFromPem(File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"/AuthConfigurations/Private.pem"));

			Key = new RsaSecurityKey(provider.ExportParameters(true));

			SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha512Signature);
		}

		public SigningConfigurations(string fileLocation) {
			using RSACryptoServiceProvider provider = new();
			provider.ImportFromPem(File.ReadAllText(fileLocation));

			Key = new RsaSecurityKey(provider.ExportParameters(true));

			SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha512Signature);
		}
	}
}