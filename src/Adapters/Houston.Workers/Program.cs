using Autofac.Extensions.DependencyInjection;
using Houston.Workers.Setups;
using Microsoft.Extensions.Configuration;

IHost host = Host.CreateDefaultBuilder(args)
	.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureServices((hostContext, services) => {
		services.AddStackExchangeRedisCache(options => {
			options.Configuration = hostContext.Configuration.GetConnectionString("Redis");
			options.InstanceName = "houston-";
		});
		services.AddAutofac();
		services.AddDependencyInjection();
		services.AddPostgres(hostContext.Configuration);
		services.AddEventBus(hostContext.Configuration);
	})
	.Build();

host.Services.ConfigureEventBus();
await host.RunAsync();