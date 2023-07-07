using Houston.Application.ViewModel.PipelineTriggerFilterViewModels;

namespace Houston.Application.ViewModel.PipelineTriggerEventViewModels {
	public class PipelineTriggerEventViewModel {
		public Guid Id { get; set; }

		public Guid PipelineTriggerId { get; set; }

		public Guid TriggerEventId { get; set; }

		public string TriggerEvent { get; set; } = null!;

		public List<PipelineTriggerFilterViewModel> PipelineTriggerFilters { get; set; } = new List<PipelineTriggerFilterViewModel>();
	}
}
