import { ConnectorFunctionSummaryViewModel } from "./connector-function-summary.view-model";

export interface ConnectorFunctionGroupedViewModel {
	friendlyName: string;
	name: string;
	connector: string;
	versions: ConnectorFunctionSummaryViewModel[];
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
