import { ConnectorFunctionViewModel } from "./connector-function.view-model";

export interface ConnectorViewModel {
	id: string;
	name: string;
	description: string;
	connectorFunctions: ConnectorFunctionViewModel[];
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
