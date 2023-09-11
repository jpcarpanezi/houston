// Original file: proto/houston/v1/pipeline.proto


export interface RunPipelineRequest {
	'files'?: (Buffer | Uint8Array | string);
	'scripts'?: (string)[];
}

export interface RunPipelineRequest__Output {
	'files': (Buffer);
	'scripts': (string)[];
}
