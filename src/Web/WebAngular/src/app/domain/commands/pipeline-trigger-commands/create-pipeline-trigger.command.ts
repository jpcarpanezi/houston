import { CreatePipelineTriggerEventsCommand } from "./create-pipeline-trigger-events.command";

export interface CreatePipelineTriggerCommand {
	pipelineId: string;
	sourceGit: string;
	secret: string;
	events: CreatePipelineTriggerEventsCommand[];
}
