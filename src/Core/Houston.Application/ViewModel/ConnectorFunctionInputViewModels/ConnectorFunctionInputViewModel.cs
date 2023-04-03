using Houston.Core.Enums;

namespace Houston.Application.ViewModel.ConnectorFunctionInputViewModels {
	public class ConnectorFunctionInputViewModel {
		public Guid Id { get; set; }

		public Guid ConnectorFunctionId { get; set; }

		public string Name { get; set; } = null!;

		public string Placeholder { get; set; } = null!;

		public InputTypeEnum Type { get; set; }

		public bool Required { get; set; }

		public string Replace { get; set; } = null!;

		public string[]? Values { get; set; }

		public string? DefaultValue { get; set; }

		public bool AdvancedOption { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
