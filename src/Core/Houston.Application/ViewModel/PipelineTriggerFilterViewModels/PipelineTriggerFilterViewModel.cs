namespace Houston.Application.ViewModel.PipelineTriggerFilterViewModels {
	public class PipelineTriggerFilterViewModel {
		public Guid Id { get; set; }

		public Guid PipelineTriggerEventId { get; set; }

		public Guid TriggerFilterId { get; set; }

		public string TriggerFilter { get; set; } = null!;

		public string[]? FilterValues { get; set; }
	}
}
