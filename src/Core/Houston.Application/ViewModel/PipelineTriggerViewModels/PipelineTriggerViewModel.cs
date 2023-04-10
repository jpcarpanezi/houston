using Houston.Application.ViewModel.PipelineTriggerEventViewModels;
using Houston.Core.Entities.Postgres;

namespace Houston.Application.ViewModel.PipelineTriggerViewModels {
	public class PipelineTriggerViewModel {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public string SourceGit { get; set; } = null!;

		public string? DeployKey { get; set; }

		public string? PublicKey { get; set; }

		public List<PipelineTriggerEventViewModel> PipelineTriggerEvents { get; set; } = new List<PipelineTriggerEventViewModel>();
	}
}
