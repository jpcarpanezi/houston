using RabbitMQ.Client;

namespace EventBus.EventBusRabbitMQ {
	public interface IRabbitMQPersistentConnection
        : IDisposable {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
