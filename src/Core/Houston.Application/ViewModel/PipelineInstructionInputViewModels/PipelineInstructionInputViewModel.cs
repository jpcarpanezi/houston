namespace Houston.Application.ViewModel.PipelineInstructionInputViewModels {
	public class PipelineInstructionInputViewModel {
		public Guid Id { get; set; }

		public Guid InputId { get; set; }

		public Guid InstructionId { get; set; }

		public string ReplaceValue { get; set; } = null!;
	}
}
