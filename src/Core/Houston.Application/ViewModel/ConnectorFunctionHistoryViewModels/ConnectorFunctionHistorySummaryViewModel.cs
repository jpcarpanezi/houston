namespace Houston.Application.ViewModel.ConnectorFunctionHistoryViewModels {
	public class ConnectorFunctionHistorySummaryViewModel {
		public Guid Id { get; set; }

		public Guid ConnectorFunctionId { get; set; }

		public string Version { get; set; } = null!;

		public BuildStatus BuildStatus { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
