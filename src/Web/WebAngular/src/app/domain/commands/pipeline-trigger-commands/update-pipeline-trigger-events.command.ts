import { UpdatePipelineTriggerEventFiltersCommand } from "./update-pipeline-trigger-event-filters.command";

export interface UpdatePipelineTriggerEventsCommand {
	triggerEventId: string;
	eventFilters: UpdatePipelineTriggerEventFiltersCommand[];
}
