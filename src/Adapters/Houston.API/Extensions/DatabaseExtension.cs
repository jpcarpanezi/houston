namespace Houston.API.Setups {
	public static class DatabaseExtension {
		public static void AddPostgres(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env) {
			services.AddDbContext<PostgresContext>(opts => {
				opts.UseNpgsql(configuration.GetConnectionString("Postgres"), x => x.MigrationsAssembly("Houston.API"));
				opts.EnableSensitiveDataLogging(env.IsDevelopment());
			});
		}

		public static IServiceCollection AddRepositories(this IServiceCollection services) {
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IUserClaimsService, UserClaimsService>();

			return services;
		}

		public static void UseMigrations(this WebApplication app) {
			using var scope = app.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<PostgresContext>();
			
			if (context.Database.GetPendingMigrations().Any()) {
				context.Database.Migrate();
			}
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
