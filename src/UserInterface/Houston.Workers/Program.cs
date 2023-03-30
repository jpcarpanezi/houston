using Autofac.Extensions.DependencyInjection;
using Houston.Core.Interfaces.Repository;
using Houston.Core.Interfaces.Services;
using Houston.Infrastructure.Context;
using Houston.Infrastructure.Repository;
using Houston.Infrastructure.Services;
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
		services.AddEventBus(hostContext.Configuration);
		services.AddScoped<IContainerBuilderService, DockerContainerBuilderService>();
		services.AddTransient<IUnitOfWork, UnitOfWork>();
		//services.AddScoped<RunPipelineEventHandler>();
	})
	.Build();

// DEBUG: Run pipeline for testing without event
//await host.Services.GetRequiredService<RunPipelineEventHandler>().Handle();
host.Services.ConfigureEventBus();
await host.RunAsync();