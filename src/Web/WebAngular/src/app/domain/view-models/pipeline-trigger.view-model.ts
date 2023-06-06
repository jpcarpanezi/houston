import { PipelineTriggerEventViewModel } from "./pipeline-trigger-event.view-model";

export interface PipelineTriggerViewModel {
	id: string;
	pipelineId: string;
	deployKey: string;
	publicKey: string;
	pipelineTriggerEvents: PipelineTriggerEventViewModel[];
}
