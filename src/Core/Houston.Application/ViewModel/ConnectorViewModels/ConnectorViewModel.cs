namespace Houston.Application.ViewModel.ConnectorViewModels {
	public class ConnectorViewModel {
		public Guid Id {  get; set; }

		public string Name { get; set; } = null!;

		public string FriendlyName { get; set; } = null!;

		public string? Description { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate {  get; set; }
	}
}
