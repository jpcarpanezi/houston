namespace Houston.Core.Exceptions {
	[Serializable]
	public class ContainerBuilderException : Exception {
		public string? Stdout { get; private set; }

		public ContainerBuilderException() { }

		public ContainerBuilderException(string message) : base(message) { }

		public ContainerBuilderException(string message, Exception inner) : base(message, inner) { }

		public ContainerBuilderException(string message, string stdout) : base(message) {
			Stdout = stdout;
		}
	}
}
