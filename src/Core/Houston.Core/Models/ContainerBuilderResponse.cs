using MongoDB.Bson;

namespace Houston.Core.Models {
	public class ContainerBuilderResponse {
		public long ExitCode { get; set; }

		public string Stdout { get; set; } = null!;

		public ObjectId? InstructionWithError { get; set; }

		public ContainerBuilderResponse() { }

		public ContainerBuilderResponse(long exitCode, string stdout, ObjectId? instructionWithError) {
			ExitCode = exitCode;
			Stdout = stdout ?? throw new ArgumentNullException(nameof(stdout));
			InstructionWithError = instructionWithError;
		}
	}
}
