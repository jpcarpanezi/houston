export interface PipelineInstructionCommand {
	connectorFunctionId: string;
	connectedToArrayIndex: number | null;
	inputs: { [key: string]: string | null };
}
