import { PipelineInstructionInputViewModel } from "./pipeline-instruction-input.view-model";

export interface PipelineInstructionViewModel {
	id: string;
	pipelineId: string;
	connectorFunctionHistoryId: string;
	connectedToArrayIndex: number | null;
	pipelineInstructionInputs: PipelineInstructionInputViewModel[];
}
