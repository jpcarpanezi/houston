import { ConnectorFunctionInputViewModel } from "./connector-function-input.view-model";

export interface ConnectorFunctionViewModel {
	id: string;
	name: string;
	description: string | null;
	active: boolean;
	connectorId: string;
	script: string[];
	inputs: ConnectorFunctionInputViewModel[] | null;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
