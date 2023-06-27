namespace Houston.Application.ViewModel.PipelineTriggerViewModels {
	public class PipelineTriggerKeysViewModel {
		public Guid Id { get; set; }

		public bool KeyRevealed { get; set; }

		public string PrivateKey { get; set; } = null!;

		public string PublicKey { get; set; } = null!;
	}
}
