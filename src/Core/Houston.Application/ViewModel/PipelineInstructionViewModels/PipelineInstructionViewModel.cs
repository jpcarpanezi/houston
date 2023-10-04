namespace Houston.Application.ViewModel.PipelineInstructionViewModels {
	public class PipelineInstructionViewModel {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public Guid ConnectorFunctionHistoryId { get; set; }

		public int? ConnectedToArrayIndex { get; set; }

		public List<PipelineInstructionInputViewModel> PipelineInstructionInputs { get; set; } = new();
	}
}
