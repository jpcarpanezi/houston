// Original file: proto/houston/v1/pipeline.proto


export interface RunPipelineInstruction {
	'script'?: (string);
	'has_error'?: (boolean);
	'stdout'?: (string);
	'stderr'?: (string);
	'_stdout'?: "stdout";
	'_stderr'?: "stderr";
}

export interface RunPipelineInstruction__Output {
	'script': (string);
	'has_error': (boolean);
	'stdout'?: (string);
	'stderr'?: (string);
	'_stdout': "stdout";
	'_stderr': "stderr";
}
