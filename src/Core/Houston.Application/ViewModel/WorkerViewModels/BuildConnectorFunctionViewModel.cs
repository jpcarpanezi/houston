namespace Houston.Application.ViewModel.WorkerViewModels {
	public class BuildConnectorFunctionViewModel {
		public int ExitCode { get; set; }

		public byte[] Dist { get; set; } = null!;

		public string Type { get; set; } = null!;

		public string? Stderr { get; set; }
	}
}
