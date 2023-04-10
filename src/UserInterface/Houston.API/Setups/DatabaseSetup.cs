﻿using Houston.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Houston.API.Setups {
	public static class DatabaseSetup {
		public static void AddPostgres(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env) {
			services.AddDbContext<PostgresContext>(opts => {
				opts.UseNpgsql(configuration.GetConnectionString("Postgres"));
				opts.EnableSensitiveDataLogging(env.IsDevelopment());
			});
		}
	}
}
