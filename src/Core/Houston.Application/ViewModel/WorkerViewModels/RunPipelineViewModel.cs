namespace Houston.Application.ViewModel.WorkerViewModels {
	public class RunPipelineViewModel {
		public long ExitCode { get; set; }

		public string Stdout { get; set; } = string.Empty;

		public Guid? InstructionWithError { get; set; }
	}
}
