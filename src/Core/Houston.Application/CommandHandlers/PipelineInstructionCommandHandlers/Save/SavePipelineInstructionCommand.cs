namespace Houston.Application.CommandHandlers.PipelineInstructionCommandHandlers.Save {
	public sealed record SavePipelineInstructionCommand(Guid PipelineId, List<SavePipelineInstruction> PipelineInstructions) : IRequest<IResultCommand>;

	public sealed record SavePipelineInstruction(Guid ConnectorFunctionId, int? ConnectedToArrayIndex, Dictionary<Guid, string?> Inputs);
}
