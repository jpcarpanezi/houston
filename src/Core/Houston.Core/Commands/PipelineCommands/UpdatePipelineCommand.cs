using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class UpdatePipelineCommand : IRequest<ResultCommand<Pipeline>> {
		public Guid Id { get; set; }
		
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public UpdatePipelineCommand() { }

		public UpdatePipelineCommand(Guid id, string name, string? description) {
			Id = id;
			Name = name ?? throw new ArgumentNullException(nameof(name));
			Description = description;
		}
	}
}
