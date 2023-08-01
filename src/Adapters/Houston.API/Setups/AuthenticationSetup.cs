namespace Houston.API.Setups {
	public static class AuthenticationSetup {
		public static void AddBearerAuthentication(this IServiceCollection services, IConfiguration configuration) {
			SigningConfigurations signingConfigurations = new();
			services.AddSingleton(signingConfigurations);

			TokenConfigurations tokenConfigurations = new();
			new ConfigureFromConfigurationOptions<TokenConfigurations>(
				configuration.GetSection("TokenConfigurations"))
					.Configure(tokenConfigurations);
			services.AddSingleton(tokenConfigurations);

			services.AddAuthentication(x => {
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x => {
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters {
					IssuerSigningKey = signingConfigurations.Key,
					ValidAudience = tokenConfigurations.Audience,
					ValidIssuer = tokenConfigurations.Issuer,
					ValidateIssuerSigningKey = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			});
		}
	}
}
