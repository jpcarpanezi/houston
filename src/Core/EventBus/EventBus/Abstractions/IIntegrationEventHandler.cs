using EventBus.EventBus.Events;

namespace EventBus.EventBus.Abstractions {
	public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
		where TIntegrationEvent : IntegrationEvent {
		Task Handle(TIntegrationEvent @event);
	}

	public interface IIntegrationEventHandler {
	}
}
