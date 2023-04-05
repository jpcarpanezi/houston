namespace Houston.Application.ViewModel {
	public class MessageViewModel {
		public string Message { get; private set; }

		public MessageViewModel(string message) {
			Message = message ?? throw new ArgumentNullException(nameof(message));
		}
	}
}
