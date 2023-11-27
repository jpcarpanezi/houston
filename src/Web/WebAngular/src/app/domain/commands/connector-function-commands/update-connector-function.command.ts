import { CreateConnectorFunctionCommand } from "./create-connector-function.command";

export interface UpdateConnectorFunctionCommand extends CreateConnectorFunctionCommand {
	id: string;
}
