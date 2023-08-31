namespace Houston.Workers.Extensions {
	public static class DatabaseExtensions {
		public static void AddPostgres(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env) {
			services.AddDbContext<PostgresContext>(opts => {
				opts.UseNpgsql(configuration.GetConnectionString("Postgres"));
				opts.EnableSensitiveDataLogging(env.IsDevelopment());
			});
		}

		public static IServiceCollection AddRepositories(this IServiceCollection services) {
			services.AddTransient<IUnitOfWork, UnitOfWork>();

			return services;
		}

		public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration) {
			services.AddStackExchangeRedisCache(options => {
				options.Configuration = configuration.GetConnectionString("Redis");
				options.InstanceName = "houston-";
			});

			return services;
		}
	}
}
