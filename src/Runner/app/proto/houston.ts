import type * as grpc from '@grpc/grpc-js';
import type { MessageTypeDefinition } from '@grpc/proto-loader';

import type { GitServiceClient as _houston_v1_git_GitServiceClient, GitServiceDefinition as _houston_v1_git_GitServiceDefinition } from './houston/v1/git/GitService';
import type { PipelineServiceClient as _houston_v1_pipeline_PipelineServiceClient, PipelineServiceDefinition as _houston_v1_pipeline_PipelineServiceDefinition } from './houston/v1/pipeline/PipelineService';

type SubtypeConstructor<Constructor extends new (...args: any) => any, Subtype> = {
	new(...args: ConstructorParameters<Constructor>): Subtype;
};

export interface ProtoGrpcType {
	houston: {
		v1: {
			git: {
				CloneRepositoryRequest: MessageTypeDefinition
				CloneRepositoryResponse: MessageTypeDefinition
				GitService: SubtypeConstructor<typeof grpc.Client, _houston_v1_git_GitServiceClient> & { service: _houston_v1_git_GitServiceDefinition }
			}
			pipeline: {
				BuildConnectorFunctionRequest: MessageTypeDefinition
				BuildConnectorFunctionResponse: MessageTypeDefinition
				PipelineService: SubtypeConstructor<typeof grpc.Client, _houston_v1_pipeline_PipelineServiceClient> & { service: _houston_v1_pipeline_PipelineServiceDefinition }
				RunPipelineInstruction: MessageTypeDefinition
				RunPipelineRequest: MessageTypeDefinition
				RunPipelineResponse: MessageTypeDefinition
			}
		}
	}
}

