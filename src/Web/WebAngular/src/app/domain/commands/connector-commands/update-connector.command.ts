export interface UpdateConnectorCommand {
	connectorId: string;
	name: string;
	description: string | null;
}
