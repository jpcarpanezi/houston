using Houston.Application.ViewModel.PipelineInstructionInputViewModels;

namespace Houston.Application.ViewModel.PipelineInstructionViewModels {
	public class PipelineInstructionViewModel {
		public Guid Id { get; set; }

		public Guid PipelineId { get; set; }

		public Guid ConnectorFunctionId { get; set; }

		public Guid? Connection { get; set; }

		public int? ConnectedToArrayIndex { get; set; }

		public string[] Script { get; set; } = null!;

		public List<PipelineInstructionInputViewModel> PipelineInstructionInputs { get; set; } = new();
	}
}
