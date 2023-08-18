using Houston.Core.Messages;
using MassTransit;

namespace Houston.Workers.Consumers {
	public class RunPipelineConsumer : IConsumer<RunPipelineMessage> {
		public async Task Consume(ConsumeContext<RunPipelineMessage> context) {
			Console.WriteLine(context.Message);

			// TODO: Download da imagem do Docker (pegar username e password do Redis)
			// TODO: Criar container
			// TODO: Extrair arquivos .JS dentro do container
			// TODO: Clonar repositório via deploy key
			// TODO: Executar a chamada da pipeline via gRPC
		}
	}
}
