using Houston.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Houston.Workers.Setups {
	public static class DatabaseSetup {
		public static void AddPostgres(this IServiceCollection services, IConfiguration configuration) {
			services.AddDbContext<PostgresContext>(opts => {
				opts.UseNpgsql(configuration.GetConnectionString("Postgres"));
			});
		}
	}
}
