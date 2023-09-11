using Houston.Core.Messages;
using MassTransit;

namespace Houston.Workers.Consumers {
	public class RunPipelineConsumer : IConsumer<RunPipelineMessage> {
		public async Task Consume(ConsumeContext<RunPipelineMessage> context) {
			Console.WriteLine(context.Message);
		}
	}
}
