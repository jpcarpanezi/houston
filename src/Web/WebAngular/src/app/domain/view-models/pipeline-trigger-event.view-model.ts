import { PipelineTriggerFilterViewModel } from "./pipeline-trigger-filter.view-model";

export interface PipelineTriggerEventViewModel {
	id: string;
	pipelineTriggerId: string;
	triggerEventId: string;
	pipelineTriggerFilters: PipelineTriggerFilterViewModel[];
}
