using Autofac.Extensions.DependencyInjection;
using Houston.Workers.Consumers;
using Houston.Workers.Setups;
using MassTransit;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Reflection;

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

		services.AddMassTransit(x => {
			x.AddConsumers(Assembly.GetExecutingAssembly());
			
			x.UsingRabbitMq((ctx, cfg) => {
				cfg.Host(hostContext.Configuration.GetConnectionString("RabbitMQ"));
				cfg.ReceiveEndpoint("Houston.Workers", e => {
					e.ExchangeType = ExchangeType.Topic;
					e.ConfigureConsumers(ctx);
				});
			});
		});
	})
	.Build();

await host.RunAsync();