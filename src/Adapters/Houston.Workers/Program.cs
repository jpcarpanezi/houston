IHost host = Host.CreateDefaultBuilder(args)
	.UseServiceProviderFactory(new AutofacServiceProviderFactory())
	.ConfigureServices((hc, services) => {
		services.AddRedis(hc.Configuration);

		services.AddAutofac();

		services.AddPostgres(hc.Configuration, hc.HostingEnvironment);

		services.AddRepositories();

		services.AddMediatR(ExtensionOptions.ConfigureMediatR);

		services.AddDependencyInjections();

		services.AddMassTransit(x => ExtensionOptions.ConfigureMassTransit(x, hc.Configuration));
	})
	.UseSerilog(ExtensionOptions.ConfigureSerilog)
	.Build();

await host.RunAsync();