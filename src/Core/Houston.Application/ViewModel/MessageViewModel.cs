namespace Houston.Application.ViewModel {
	public class MessageViewModel {
		public string Message { get; private set; }

		public string? ErrorCode { get; private set; }

		public MessageViewModel(string message, string? errorCode = null) {
			Message = message ?? throw new ArgumentNullException(nameof(message));
			ErrorCode = errorCode;
		}
	}
}
