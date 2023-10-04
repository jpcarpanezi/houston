namespace Houston.Application.ViewModel.ConnectorFunctionHistoryViewModels {
	public class ConnectorFunctionHistoryDetailViewModel {
		public Guid Id { get; set; }

		public Guid ConnectorFunctionId { get; set; }

		public string Version { get; set; } = null!;

		public byte[] Script { get; set; } = null!;

		public byte[] Package { get; set; } = null!;

		public List<ConnectorFunctionInputViewModel> Inputs { get; set; } = new List<ConnectorFunctionInputViewModel>();

		public BuildStatus BuildStatus { get; set; }

		public byte[]? BuildStderr { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
