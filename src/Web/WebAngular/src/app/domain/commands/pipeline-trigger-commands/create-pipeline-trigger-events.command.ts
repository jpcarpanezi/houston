import { CreatePipelineTriggerEventFiltersCommand } from "./create-pipeline-trigger-event-filters.command";

export interface CreatePipelineTriggerEventsCommand {
	triggerEventId: string;
	eventFilters: CreatePipelineTriggerEventFiltersCommand[];
}
