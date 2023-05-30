import { PipelineStatus } from "../enums/pipeline-status.enum";

export interface PipelineViewModel {
	id: string;
	name: string;
	description: string | null;
	status: PipelineStatus;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
