namespace Houston.Application.ViewModel.ConnectorFunctionViewModels {
	public class ConnectorFunctionGroupedViewModel {
		public string FriendlyName { get; set; } = null!;
		
		public string Name { get; set; } = null!;

		public string Connector { get; set; } = null!;

		public List<ConnectorFunctionSummaryViewModel> Versions { get; set; } = new();

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
