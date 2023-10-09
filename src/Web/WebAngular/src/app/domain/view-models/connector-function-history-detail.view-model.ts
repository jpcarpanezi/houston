import { BuildStatus } from "../enums/build-status.enum";
import { ConnectorFunctionInputViewModel } from "./connector-function-input.view-model";

export interface ConnectorFunctionHistoryDetailViewModel {
	id: string;
	connectorFunctionId: string;
	version: string;
	script: Buffer | Uint8Array | string;
	package: Buffer | Uint8Array | string;
	inputs: ConnectorFunctionInputViewModel[];
	buildStatus: BuildStatus;
	buildStderr: Buffer | Uint8Array | string | null;
	createdBy: string;
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
