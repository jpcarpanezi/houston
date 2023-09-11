export interface PipelineInstructionCommand {
	connectorFunctionId: string;
	connectedToArrayIndex: number | null;
	script: Buffer;
	inputs: { [key: string]: string | null };
}
