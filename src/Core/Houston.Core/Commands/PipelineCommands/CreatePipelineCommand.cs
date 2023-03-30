using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class CreatePipelineCommand : IRequest<ResultCommand<Pipeline>> {
		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public Guid RepositoryHostConfig { get; set; }

		public string SourceCode { get; set; } = null!;

		public string? DeployKey { get; set; } = null!;

		public string? Secret { get; set; } = null!;

		public List<PipelineTrigger> Triggers { get; set; } = null!;
	}
}
