namespace Houston.Application.ViewModel {
	public class LockedMessageViewModel {
		public string Message { get; private set; }

		public DateTime EstimatedCompletionTime { get; private set; }

		public LockedMessageViewModel(string message, DateTime estimatedCompletionTime) {
			Message = message ?? throw new ArgumentNullException(nameof(message));
			EstimatedCompletionTime = estimatedCompletionTime;
		}
	}
}
