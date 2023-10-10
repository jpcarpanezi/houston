import { ConnectorFunctionHistorySummaryViewModel } from "./connector-function-history-summary.view-model";

export interface ConnectorFunctionViewModel {
	id: string;
	name: string;
	description: string | null;
	active: boolean;
	versions: ConnectorFunctionHistorySummaryViewModel[];
	connectorId: string;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
