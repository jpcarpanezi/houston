// Original file: proto/houston/v1/pipeline.proto


export interface BuildConnectorFunctionResponse {
	'exit_code'?: (number);
	'dist'?: (Buffer | Uint8Array | string);
	'type'?: (string);
	'stderr'?: (string);
	'_dist'?: "dist";
	'_type'?: "type";
	'_stderr'?: "stderr";
}

export interface BuildConnectorFunctionResponse__Output {
	'exit_code': (number);
	'dist'?: (Buffer);
	'type'?: (string);
	'stderr'?: (string);
	'_dist': "dist";
	'_type': "type";
	'_stderr': "stderr";
}
