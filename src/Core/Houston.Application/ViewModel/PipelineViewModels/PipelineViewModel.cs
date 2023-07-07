using Houston.Core.Enums;

namespace Houston.Application.ViewModel.PipelineViewModels {
	public class PipelineViewModel {
		public Guid Id { get; set; }

		public string Name { get; set; } = null!;

		public string? Description { get; set; }

		public PipelineStatusEnum Status { get; set; }

		public string CreatedBy { get; set; } = null!;

		public DateTime CreationDate { get; set; }

		public string UpdatedBy { get; set; } = null!;

		public DateTime LastUpdate { get; set; }
	}
}
