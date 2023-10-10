import { BuildStatus } from "../enums/build-status.enum";

export interface ConnectorFunctionHistorySummaryViewModel {
	id: string;
	connectorFunctionId: string;
	version: string;
	buildStatus: BuildStatus;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
