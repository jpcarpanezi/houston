import { BuildStatus } from "../enums/build-status.enum";
import { ConnectorFunctionInputViewModel } from "./connector-function-input.view-model";

export interface ConnectorFunctionViewModel {
	id: string;
	name: string;
	description: string | null;
	active: boolean;
	connectorId: string;
	script: Buffer | Uint8Array | string;
	package: Buffer | Uint8Array | string;
	version: string;
	buildStatus: BuildStatus;
	buildStderr: Buffer | Uint8Array | string | null;
	inputs: ConnectorFunctionInputViewModel[] | null;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
