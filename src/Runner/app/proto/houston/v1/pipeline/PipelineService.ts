// Original file: proto/houston/v1/pipeline.proto

import type * as grpc from '@grpc/grpc-js'
import type { MethodDefinition } from '@grpc/proto-loader'
import type { BuildConnectorFunctionRequest as _houston_v1_pipeline_BuildConnectorFunctionRequest, BuildConnectorFunctionRequest__Output as _houston_v1_pipeline_BuildConnectorFunctionRequest__Output } from '../../../houston/v1/pipeline/BuildConnectorFunctionRequest';
import type { BuildConnectorFunctionResponse as _houston_v1_pipeline_BuildConnectorFunctionResponse, BuildConnectorFunctionResponse__Output as _houston_v1_pipeline_BuildConnectorFunctionResponse__Output } from '../../../houston/v1/pipeline/BuildConnectorFunctionResponse';
import type { RunPipelineRequest as _houston_v1_pipeline_RunPipelineRequest, RunPipelineRequest__Output as _houston_v1_pipeline_RunPipelineRequest__Output } from '../../../houston/v1/pipeline/RunPipelineRequest';
import type { RunPipelineResponse as _houston_v1_pipeline_RunPipelineResponse, RunPipelineResponse__Output as _houston_v1_pipeline_RunPipelineResponse__Output } from '../../../houston/v1/pipeline/RunPipelineResponse';

export interface PipelineServiceClient extends grpc.Client {
	BuildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	BuildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	BuildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	BuildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	buildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	buildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	buildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;
	buildConnectorFunction(argument: _houston_v1_pipeline_BuildConnectorFunctionRequest, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorFunctionResponse__Output>): grpc.ClientUnaryCall;

	RunPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	RunPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	RunPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	RunPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	runPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	runPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	runPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;
	runPipeline(argument: _houston_v1_pipeline_RunPipelineRequest, callback: grpc.requestCallback<_houston_v1_pipeline_RunPipelineResponse__Output>): grpc.ClientUnaryCall;

}

export interface PipelineServiceHandlers extends grpc.UntypedServiceImplementation {
	BuildConnectorFunction: grpc.handleUnaryCall<_houston_v1_pipeline_BuildConnectorFunctionRequest__Output, _houston_v1_pipeline_BuildConnectorFunctionResponse>;

	RunPipeline: grpc.handleUnaryCall<_houston_v1_pipeline_RunPipelineRequest__Output, _houston_v1_pipeline_RunPipelineResponse>;

}

export interface PipelineServiceDefinition extends grpc.ServiceDefinition {
	BuildConnectorFunction: MethodDefinition<_houston_v1_pipeline_BuildConnectorFunctionRequest, _houston_v1_pipeline_BuildConnectorFunctionResponse, _houston_v1_pipeline_BuildConnectorFunctionRequest__Output, _houston_v1_pipeline_BuildConnectorFunctionResponse__Output>
	RunPipeline: MethodDefinition<_houston_v1_pipeline_RunPipelineRequest, _houston_v1_pipeline_RunPipelineResponse, _houston_v1_pipeline_RunPipelineRequest__Output, _houston_v1_pipeline_RunPipelineResponse__Output>
}
