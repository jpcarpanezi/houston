import { UpdatePipelineTriggerEventsCommand } from "./update-pipeline-trigger-events.command";

export interface UpdatePipelineTriggerCommand {
	pipelineTriggerId: string;
	sourceGit: string;
	events: UpdatePipelineTriggerEventsCommand[];
}
