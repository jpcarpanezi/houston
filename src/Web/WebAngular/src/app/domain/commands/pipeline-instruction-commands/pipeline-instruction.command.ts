export interface PipelineInstructionCommand {
	connectorFunctionHistoryId: string;
	connectedToArrayIndex: number | null;
	connectorFunctionId: string;
	inputs: { [key: string]: string | null };
}
