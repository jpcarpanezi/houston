// Original file: proto/houston/v1/pipeline.proto

import type * as grpc from '@grpc/grpc-js'
import type { MethodDefinition } from '@grpc/proto-loader'
import type { BuildConnectorRequest as _houston_v1_pipeline_BuildConnectorRequest, BuildConnectorRequest__Output as _houston_v1_pipeline_BuildConnectorRequest__Output } from '../../../houston/v1/pipeline/BuildConnectorRequest';
import type { BuildConnectorResponse as _houston_v1_pipeline_BuildConnectorResponse, BuildConnectorResponse__Output as _houston_v1_pipeline_BuildConnectorResponse__Output } from '../../../houston/v1/pipeline/BuildConnectorResponse';
import type { RunPipelineRequest as _houston_v1_pipeline_RunPipelineRequest, RunPipelineRequest__Output as _houston_v1_pipeline_RunPipelineRequest__Output } from '../../../houston/v1/pipeline/RunPipelineRequest';
import type { RunPipelineResponse as _houston_v1_pipeline_RunPipelineResponse, RunPipelineResponse__Output as _houston_v1_pipeline_RunPipelineResponse__Output } from '../../../houston/v1/pipeline/RunPipelineResponse';

export interface PipelineServiceClient extends grpc.Client {
	BuildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	BuildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	BuildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	BuildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	buildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	buildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	buildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;
	buildConnector(argument: _houston_v1_pipeline_BuildConnectorRequest, callback: grpc.requestCallback<_houston_v1_pipeline_BuildConnectorResponse__Output>): grpc.ClientUnaryCall;

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
	BuildConnector: grpc.handleUnaryCall<_houston_v1_pipeline_BuildConnectorRequest__Output, _houston_v1_pipeline_BuildConnectorResponse>;

	RunPipeline: grpc.handleUnaryCall<_houston_v1_pipeline_RunPipelineRequest__Output, _houston_v1_pipeline_RunPipelineResponse>;

}

export interface PipelineServiceDefinition extends grpc.ServiceDefinition {
	BuildConnector: MethodDefinition<_houston_v1_pipeline_BuildConnectorRequest, _houston_v1_pipeline_BuildConnectorResponse, _houston_v1_pipeline_BuildConnectorRequest__Output, _houston_v1_pipeline_BuildConnectorResponse__Output>
	RunPipeline: MethodDefinition<_houston_v1_pipeline_RunPipelineRequest, _houston_v1_pipeline_RunPipelineResponse, _houston_v1_pipeline_RunPipelineRequest__Output, _houston_v1_pipeline_RunPipelineResponse__Output>
}
