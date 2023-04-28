namespace Houston.Core.Models {
	public class ContainerChainResponse {
		public long ExitCode { get; set; }

		public string Stdout { get; set; } = null!;

		public Guid? InstructionWithError { get; set; }

		public ContainerChainResponse() { }

		public ContainerChainResponse(long exitCode, string stdout, Guid? instructionWithError) {
			ExitCode = exitCode;
			Stdout = stdout ?? throw new ArgumentNullException(nameof(stdout));
			InstructionWithError = instructionWithError;
		}
	}
}
