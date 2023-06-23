export interface PipelineLogViewModel {
	id: string;
	pipelineId: string;
	exitCode: number;
	runN: number;
	stdout: string | null;
	instructionWithErrorId: string | null;
	instructionWithError: string;
	triggeredBy: string;
	startTime: string;
	duration: string;
}
