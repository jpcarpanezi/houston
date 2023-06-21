export interface PipelineInstructionCommand {
	connectorFunctionId: string;
	connectedToArrayIndex: number | null;
	script: string[];
	inputs: { [key: string]: string | null };
}
