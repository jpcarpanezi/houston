using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class CreatePipelineCommand : IRequest<ResultCommand<Pipeline>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public CreatePipelineCommand() { }

		public CreatePipelineCommand(string name, string? description) {
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
		}
	}
}
