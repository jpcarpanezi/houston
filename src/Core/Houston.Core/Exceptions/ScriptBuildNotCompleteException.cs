namespace Houston.Core.Exceptions {
	[Serializable]
	public class ScriptBuildNotCompleteException : Exception {
		public BuildStatus BuildStatus { get; private set; }

		public string ConnectorFunctionName { get; private set; }

		public Guid ConnectorFunctionId { get; private set; }

		public Guid InstructionId { get; private set; }


		public ScriptBuildNotCompleteException(string message, BuildStatus buildStatus, Guid instructionId, Guid connectorFunctionId, string connectorFunctionName) : base(message) {
			BuildStatus = buildStatus;
			InstructionId = instructionId;
			ConnectorFunctionId = connectorFunctionId;
			ConnectorFunctionName = connectorFunctionName;
		}

		public ScriptBuildNotCompleteException(string message, Exception inner, BuildStatus buildStatus, Guid instructionId, Guid connectorFunctionId, string connectorFunctionName) : base(message, inner) {
			BuildStatus = buildStatus;
			InstructionId = instructionId;
			ConnectorFunctionId = connectorFunctionId;
			ConnectorFunctionName = connectorFunctionName;
		}
	}
}
