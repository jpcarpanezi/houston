namespace Houston.Application.Security {
	public static class TokenService {
		public async static Task<BearerTokenViewModel> GenerateToken(User user, SigningConfigurations signingConfigurations, TokenConfigurations tokenConfigurations, IDistributedCache cache) {
			DateTime creationDate = DateTime.UtcNow;
			DateTime expirationDate = creationDate + TimeSpan.FromSeconds(tokenConfigurations.Seconds);
			TimeSpan finalExpiration = TimeSpan.FromSeconds(tokenConfigurations.FinalExpiration);

			JwtSecurityTokenHandler tokenHandler = new();
			SecurityTokenDescriptor tokenDescriptor = new() {
				Subject = new ClaimsIdentity(new Claim[] {
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, user.Name),
					new Claim(ClaimTypes.Role, user.Role.ToString())
				}),
				Issuer = tokenConfigurations.Issuer,
				Audience = tokenConfigurations.Audience,
				Expires = expirationDate,
				NotBefore = creationDate,
				SigningCredentials = signingConfigurations.SigningCredentials
			};

			SecurityToken createToken = tokenHandler.CreateToken(tokenDescriptor);
			string token = tokenHandler.WriteToken(createToken);

			BearerTokenViewModel result = new("success", creationDate, expirationDate, token, Guid.NewGuid().ToString("N"));
			RefreshTokenData refreshTokenData = new(result.RefreshToken, user.Id.ToString(), user.Email);

			DistributedCacheEntryOptions cacheOptions = new();
			cacheOptions.SetAbsoluteExpiration(finalExpiration);
			await cache.SetStringAsync(result.RefreshToken, JsonSerializer.Serialize(refreshTokenData), cacheOptions);

			return result;
		}
	}
}