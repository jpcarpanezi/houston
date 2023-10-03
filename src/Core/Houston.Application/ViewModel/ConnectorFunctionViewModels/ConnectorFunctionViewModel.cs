namespace Houston.Application.ViewModel.ConnectorFunctionViewModels {
	public class ConnectorFunctionViewModel {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public bool Active { get; set; }

		public Guid ConnectorId { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
