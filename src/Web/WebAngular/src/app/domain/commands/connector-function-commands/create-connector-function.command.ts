export interface CreateConnectorFunctionCommand {
	name: string;
	description: string | null;
	connectorId: string;
}
