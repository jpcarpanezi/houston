using Houston.Core.Messages;
using MassTransit;

namespace Houston.Workers.Consumers {
	public class BuildConnectorFunctionConsumer : IConsumer<BuildConnectorFunctionMessage> {
		public Task Consume(ConsumeContext<BuildConnectorFunctionMessage> context) {
			throw new NotImplementedException();
		}
	}
}
