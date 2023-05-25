import { CreateConnectorFunctionInputCommand } from "./create-connector-function-input.command";

export interface CreateConnectorFunctionCommand {
	name: string;
	description: string | null;
	connectorId: string;
	inputs: CreateConnectorFunctionInputCommand[];
	script: string[];
}
