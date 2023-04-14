namespace Houston.Application.ViewModel {
	public class LockedMessageViewModel {
		public string Message { get; private set; }

		public string ErrorCode { get; private set; }

		public DateTime EstimatedCompletionTime { get; private set; }

		public LockedMessageViewModel(string message, string errorCode, DateTime estimatedCompletionTime) {
			Message = message ?? throw new ArgumentNullException(nameof(message));
			ErrorCode = errorCode ?? throw new ArgumentNullException(nameof(errorCode));
			EstimatedCompletionTime = estimatedCompletionTime;
		}
	}
}
