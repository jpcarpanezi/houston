import { PipelineInstructionInputViewModel } from "./pipeline-instruction-input.view-model";

export interface PipelineInstructionViewModel {
	id: string;
	pipelineId: string;
	connectorFunctionId: string;
	connection: string;
	script: string[];
	pipelineInstructionInputs: PipelineInstructionInputViewModel[];
}
