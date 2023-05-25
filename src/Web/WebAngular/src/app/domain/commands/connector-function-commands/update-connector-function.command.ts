import { UpdateConnectorFunctionInputCommand } from "./update-connector-function-input.command";

export interface UpdateConnectorFunctionCommand {
	connectorFunctionId: string;
	name: string;
	description: string | null;
	connectorId: string;
	inputs: UpdateConnectorFunctionInputCommand[];
	script: string[];
}
