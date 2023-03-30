using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineCommands {
	public class UpdatePipelineInstructionsCommand : IRequest<ResultCommand<List<PipelineInstruction>>> {
		public Guid PipelineId { get; set; }

		public List<UpdatePipelineInstructionsListCommand> PipelineInstructions { get; set; } = null!;
	}

	public class UpdatePipelineInstructionsListCommand {
		public Guid ConnectorFunctionId { get; set; }

		public string? Comments { get; set; }

		public uint InterfaceId { get; set; }

		public List<uint>? Connections { get; set; } = null!;

		public uint PosX { get; set; }

		public uint PosY { get; set; }

		public List<PipelineInstructionInput>? Inputs { get; set; } = null!;	

		public List<string> Script { get; set; } = null!;
	}
}
