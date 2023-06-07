import { PipelineInstructionCommand } from "./pipeline-instruction.command";

export interface SavePipelineInstructionCommand {
	pipelineId: string;
	pipelineInstructions: PipelineInstructionCommand[];
}
