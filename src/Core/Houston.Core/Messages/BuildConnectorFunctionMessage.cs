namespace Houston.Core.Messages {
	public record class BuildConnectorFunctionMessage(byte[] Script, byte[] Package);
}
