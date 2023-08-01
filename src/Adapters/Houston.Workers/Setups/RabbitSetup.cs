using Autofac;
using EventBus.EventBus;
using EventBus.EventBus.Abstractions;
using EventBus.EventBusRabbitMQ;
using Houston.Core.Messages;
using Houston.Workers.MessageHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Houston.Workers.Setups {
	public static class RabbitSetup {
		public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration) {
			services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
				ILogger<DefaultRabbitMQPersistentConnection> logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

				int retryCount = 5;
				ConnectionFactory factory = new() {
					Uri = new Uri(configuration.GetConnectionString("RabbitMQ")!),
					DispatchConsumersAsync = true,
				};

				return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
			});

			RegisterEventBus(services);

			return services;
		}

		private static void RegisterEventBus(IServiceCollection services) {
			services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp => {
				int retryCount = 5;
				string subscriptionClientName = "Houston.Workers";
				IRabbitMQPersistentConnection rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
				ILifetimeScope iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
				ILogger<EventBusRabbitMQ> logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
				IEventBusSubscriptionsManager eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

				return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
			});

			services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

			services.AddTransient<RunPipelineMessageHandler>();
		}

		public static IServiceProvider ConfigureEventBus(this IServiceProvider app) {
			var eventBus = app.GetRequiredService<IEventBus>();

			eventBus.Subscribe<RunPipelineMessage, RunPipelineMessageHandler>();

			return app;
		}
	}
}
