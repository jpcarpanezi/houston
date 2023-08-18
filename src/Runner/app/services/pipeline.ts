import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import { ExecSyncOptionsWithStringEncoding, execSync } from "child_process";
import { RunPipelineRequest__Output } from "../proto/houston/v1/pipeline/RunPipelineRequest";
import { RunPipelineResponse } from "../proto/houston/v1/pipeline/RunPipelineResponse";
import { RunPipelineInstruction } from "../proto/houston/v1/pipeline/RunPipelineInstruction";
import { BuildConnectorRequest__Output } from "../proto/houston/v1/pipeline/BuildConnectorRequest";
import { BuildConnectorResponse } from "../proto/houston/v1/pipeline/BuildConnectorResponse";

export default class PipelineService {
	public runPipeline(call: ServerUnaryCall<RunPipelineRequest__Output, RunPipelineResponse>, callback: sendUnaryData<RunPipelineResponse>): void {
		const response: RunPipelineResponse = {
			exitCode: 0,
			instructions: []
		};

		for (const script of call.request.scripts) {
			const instruction: RunPipelineInstruction = {
				script,
				hasError: false,
				stdout: "",
				stderr: ""
			};

			try {
				const options: ExecSyncOptionsWithStringEncoding = { encoding: "utf-8", stdio: "pipe", cwd: `app/scripts/${script}/` };
				execSync("npm install", options);
				var stdout = execSync(`node ${script}.js`, options);

				instruction.stdout = stdout;
			} catch (error: any) {
				instruction.hasError = true;
				instruction.stderr = error.stderr ? error.stderr.toString() : null;
				instruction.stdout = error.stdout ? error.stdout.toString() : null;
				response.exitCode = error.status || 1;
			}

			response.instructions?.push(instruction);

			if (response.exitCode !== 0) {
				break;
			}
		}

		callback(null, response);
	}

	public buildConnector(call: ServerUnaryCall<BuildConnectorRequest__Output, BuildConnectorResponse>, callback: sendUnaryData<BuildConnectorResponse>): void {
		callback(null, null);
	}
}
