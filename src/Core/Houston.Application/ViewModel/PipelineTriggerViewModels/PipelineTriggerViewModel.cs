namespace Houston.Application.ViewModel.PipelineTriggerViewModels {
	public class PipelineTriggerViewModel {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public string SourceGit { get; set; } = null!;

		public bool KeyRevealed { get; set; }
	}
}
