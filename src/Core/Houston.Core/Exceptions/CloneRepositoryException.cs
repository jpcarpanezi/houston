namespace Houston.Core.Exceptions {
	[Serializable]
	public class CloneRepositoryException : Exception {
		public CloneRepositoryException(string message) : base(message) { }

		public CloneRepositoryException(string message, Exception inner) : base(message, inner) { }
	}
}
