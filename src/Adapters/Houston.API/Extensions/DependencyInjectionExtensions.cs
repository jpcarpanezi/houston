namespace Houston.API.Extensions {
	public static class DependencyInjectionExtensions {
		public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration) {
			services.AddOptions<Cors>()
					.Bind(configuration.GetSection("Cors"))
					.ValidateOnStart();

			return services;
		}

		public static IServiceCollection ConfigureCors(this IServiceCollection services) {
			services.AddCors(options => {
				var corsSettings = services.BuildServiceProvider().GetRequiredService<IOptions<Cors>>().Value;

				options.AddDefaultPolicy(builder => {
					builder
						.WithOrigins(corsSettings.AllowedOrigins)
						.WithMethods(corsSettings.AllowedMethods)
						.AllowAnyHeader()
						.AllowCredentials();
				});
			});

			return services;
		}
	}
}
