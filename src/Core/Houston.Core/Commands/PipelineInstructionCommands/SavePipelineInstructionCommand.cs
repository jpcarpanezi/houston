using Houston.Core.Entities.Postgres;
using MediatR;

namespace Houston.Core.Commands.PipelineInstructionCommands {
	public class SavePipelineInstructionCommand : IRequest<ResultCommand<List<PipelineInstruction>>> {
		public Guid PipelineId { get; set; }

		public List<PipelineInstruction> PipelineInstructions { get; set; } = null!;

		public SavePipelineInstructionCommand() { }

		public SavePipelineInstructionCommand(Guid pipelineId, List<PipelineInstruction> pipelineInstructions) {
			PipelineId = pipelineId;
			PipelineInstructions = pipelineInstructions ?? throw new ArgumentNullException(nameof(pipelineInstructions));
		}

		public class PipelineInstruction {
			public Guid ConnectorFunctionId { get; set; }

			public int? ConnectedToArrayIndex { get; set; }

			public string[] Script { get; set; } = null!;

			public Dictionary<Guid, string?> Inputs { get; set; } = new Dictionary<Guid, string?>();

			public PipelineInstruction() { }

			public PipelineInstruction(Guid connectorFunctionId, int? connectedToArrayIndex, string[] script, Dictionary<Guid, string?> inputs) {
				ConnectorFunctionId = connectorFunctionId;
				ConnectedToArrayIndex = connectedToArrayIndex;
				Script = script ?? throw new ArgumentNullException(nameof(script));
				Inputs = inputs ?? throw new ArgumentNullException(nameof(inputs));
			}
		}
	}
}
