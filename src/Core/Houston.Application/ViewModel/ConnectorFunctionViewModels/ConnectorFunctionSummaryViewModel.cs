namespace Houston.Application.ViewModel.ConnectorFunctionViewModels {
	public class ConnectorFunctionSummaryViewModel {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string FriendlyName { get; set; } = null!;

		public string Connector { get; set; } = null!;

		public string Version { get; set; } = null!;

		public BuildStatus BuildStatus { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
