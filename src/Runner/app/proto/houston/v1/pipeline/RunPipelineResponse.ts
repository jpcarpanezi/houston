// Original file: proto/houston/v1/pipeline.proto

import type { RunPipelineInstruction as _houston_v1_pipeline_RunPipelineInstruction, RunPipelineInstruction__Output as _houston_v1_pipeline_RunPipelineInstruction__Output } from '../../../houston/v1/pipeline/RunPipelineInstruction';

export interface RunPipelineResponse {
	'exitCode'?: (number);
	'instructions'?: (_houston_v1_pipeline_RunPipelineInstruction)[];
}

export interface RunPipelineResponse__Output {
	'exitCode': (number);
	'instructions': (_houston_v1_pipeline_RunPipelineInstruction__Output)[];
}
