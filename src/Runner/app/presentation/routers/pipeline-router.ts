import { PipelineServiceHandlers } from "../../proto/houston/v1/pipeline/PipelineService";
import { BuildConnectorRequest__Output } from "../../proto/houston/v1/pipeline/BuildConnectorRequest";
import { BuildConnectorResponse } from "../../proto/houston/v1/pipeline/BuildConnectorResponse";
import { RunPipelineRequest__Output } from "../../proto/houston/v1/pipeline/RunPipelineRequest";
import { RunPipelineResponse } from "../../proto/houston/v1/pipeline/RunPipelineResponse";
import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import PipelineService from "../../services/pipeline";

var pipelineService = new PipelineService();

export const PipelineRouter: PipelineServiceHandlers = {
	BuildConnector: function (call: ServerUnaryCall<BuildConnectorRequest__Output, BuildConnectorResponse>, callback: sendUnaryData<BuildConnectorResponse>): void {
		pipelineService.buildConnector(call, callback);
	},

	RunPipeline: function (call: ServerUnaryCall<RunPipelineRequest__Output, RunPipelineResponse>, callback: sendUnaryData<RunPipelineResponse>): void {
		pipelineService.runPipeline(call, callback);
	}
}
