import { UpdateConnectorFunctionInputCommand } from "./update-connector-function-input.command";

export interface UpdateConnectorFunctionHistoryCommand {
	id: string;
	script: Buffer | Uint8Array | string;
	package: Buffer | Uint8Array | string;
	inputs: UpdateConnectorFunctionInputCommand[];
}
