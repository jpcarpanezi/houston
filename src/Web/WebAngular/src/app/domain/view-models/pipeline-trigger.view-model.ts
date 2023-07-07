import { PipelineTriggerEventViewModel } from "./pipeline-trigger-event.view-model";

export interface PipelineTriggerViewModel {
	id: string;
	pipelineId: string;
	sourceGit: string;
	keyRevealed: boolean;
	pipelineTriggerEvents: PipelineTriggerEventViewModel[];
}
