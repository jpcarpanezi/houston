// Original file: proto/houston/v1/pipeline.proto


export interface BuildConnectorResponse {
	'exit_code'?: (number);
	'dist'?: (Buffer | Uint8Array | string);
	'stderr'?: (string);
	'_dist'?: "dist";
	'_stderr'?: "stderr";
}

export interface BuildConnectorResponse__Output {
	'exit_code': (number);
	'dist'?: (Buffer);
	'stderr'?: (string);
	'_dist': "dist";
	'_stderr': "stderr";
}
