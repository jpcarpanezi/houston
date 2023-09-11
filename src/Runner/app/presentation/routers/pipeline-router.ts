import { PipelineServiceHandlers } from "../../proto/houston/v1/pipeline/PipelineService";
import { RunPipelineRequest__Output } from "../../proto/houston/v1/pipeline/RunPipelineRequest";
import { RunPipelineResponse } from "../../proto/houston/v1/pipeline/RunPipelineResponse";
import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import PipelineService from "../../services/pipeline";
import { BuildConnectorFunctionRequest__Output } from "../../proto/houston/v1/pipeline/BuildConnectorFunctionRequest";
import { BuildConnectorFunctionResponse } from "../../proto/houston/v1/pipeline/BuildConnectorFunctionResponse";

const pipelineService = new PipelineService();

export const PipelineRouter: PipelineServiceHandlers = {
	RunPipeline: function (call: ServerUnaryCall<RunPipelineRequest__Output, RunPipelineResponse>, callback: sendUnaryData<RunPipelineResponse>): void {
		pipelineService.runPipeline(call, callback);
	},

	BuildConnectorFunction: function (call: ServerUnaryCall<BuildConnectorFunctionRequest__Output, BuildConnectorFunctionResponse>, callback: sendUnaryData<BuildConnectorFunctionResponse>): void {
		pipelineService.buildConnector(call, callback);
	}
}
