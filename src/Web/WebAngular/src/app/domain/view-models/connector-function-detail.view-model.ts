import { BuildStatus } from "../enums/build-status.enum";

export interface ConnectorFunctionDetailViewModel {
	id: string;
	name: string;
	function: string;
	friendlyName: string;
	connector: string
	version: string;
	specsFile: string;
	script: string;
	package: string;
	buildStatus: BuildStatus;
	buildStderr: string | null;
	createdBy: string
	creationDate: string;
	updatedBy: string;
	lastUpdate: string;
}
