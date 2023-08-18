// Original file: proto/houston/v1/git.proto

import type * as grpc from '@grpc/grpc-js'
import type { MethodDefinition } from '@grpc/proto-loader'
import type { CloneRepositoryRequest as _houston_v1_git_CloneRepositoryRequest, CloneRepositoryRequest__Output as _houston_v1_git_CloneRepositoryRequest__Output } from '../../../houston/v1/git/CloneRepositoryRequest';
import type { CloneRepositoryResponse as _houston_v1_git_CloneRepositoryResponse, CloneRepositoryResponse__Output as _houston_v1_git_CloneRepositoryResponse__Output } from '../../../houston/v1/git/CloneRepositoryResponse';

export interface GitServiceClient extends grpc.Client {
	CloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	CloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	CloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	CloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	cloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, metadata: grpc.Metadata, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	cloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, metadata: grpc.Metadata, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	cloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, options: grpc.CallOptions, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;
	cloneRepository(argument: _houston_v1_git_CloneRepositoryRequest, callback: grpc.requestCallback<_houston_v1_git_CloneRepositoryResponse__Output>): grpc.ClientUnaryCall;

}

export interface GitServiceHandlers extends grpc.UntypedServiceImplementation {
	CloneRepository: grpc.handleUnaryCall<_houston_v1_git_CloneRepositoryRequest__Output, _houston_v1_git_CloneRepositoryResponse>;

}

export interface GitServiceDefinition extends grpc.ServiceDefinition {
	CloneRepository: MethodDefinition<_houston_v1_git_CloneRepositoryRequest, _houston_v1_git_CloneRepositoryResponse, _houston_v1_git_CloneRepositoryRequest__Output, _houston_v1_git_CloneRepositoryResponse__Output>
}
