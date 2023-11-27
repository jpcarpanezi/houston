import { BuildStatus } from "../enums/build-status.enum";

export interface ConnectorFunctionSummaryViewModel {
	id: string;
	name: string;
	friendlyName: string;
	connector: string
	version: string;
	buildStatus: BuildStatus;
	createdBy: string
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
