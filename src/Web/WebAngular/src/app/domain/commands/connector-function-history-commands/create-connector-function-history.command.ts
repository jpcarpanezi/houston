import { CreateConnectorFunctionInputCommand } from "./create-connector-function-input.command";

export interface CreateConnectorFunctionHistoryCommand {
	connectorFunctionId: string;
	script: Buffer | Uint8Array | string;
	package: Buffer | Uint8Array | string;
	inputs: CreateConnectorFunctionInputCommand[];
}
