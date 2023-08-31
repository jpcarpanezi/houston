namespace Houston.Workers.Options {
	public static class ExtensionOptions {
		public static void ConfigureMassTransit(IBusRegistrationConfigurator configurator, IConfiguration configuration) {
			configurator.AddConsumers(Assembly.GetExecutingAssembly());

			configurator.UsingRabbitMq((ctx, cfg) => {
				cfg.Host(configuration.GetConnectionString("RabbitMQ"));
				cfg.ReceiveEndpoint("Houston.Workers", e => {
					e.ExchangeType = ExchangeType.Topic;
					e.ConfigureConsumers(ctx);
				});
			});
		}

		public static void ConfigureMediatR(MediatRServiceConfiguration options) {
			options.RegisterServicesFromAssembly(AppDomain.CurrentDomain.Load("Houston.Application"));
			options.AddOpenRequestPostProcessor(typeof(StopAndRemoveContainerBehavior<,>));
		}

		public static void ConfigureSerilog(HostBuilderContext context, LoggerConfiguration loggerConfiguration) {
			loggerConfiguration.ReadFrom.Configuration(context.Configuration);
		}
	}
}
