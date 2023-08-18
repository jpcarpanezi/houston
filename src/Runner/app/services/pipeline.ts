import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import { ExecSyncOptionsWithStringEncoding, execSync } from "child_process";
import { RunPipelineRequest__Output } from "../proto/houston/v1/pipeline/RunPipelineRequest";
import { RunPipelineResponse } from "../proto/houston/v1/pipeline/RunPipelineResponse";
import { RunPipelineInstruction } from "../proto/houston/v1/pipeline/RunPipelineInstruction";
import { BuildConnectorRequest__Output } from "../proto/houston/v1/pipeline/BuildConnectorRequest";
import { BuildConnectorResponse } from "../proto/houston/v1/pipeline/BuildConnectorResponse";
import fs from "fs";

export default class PipelineService {
	public runPipeline(call: ServerUnaryCall<RunPipelineRequest__Output, RunPipelineResponse>, callback: sendUnaryData<RunPipelineResponse>): void {
		let response: RunPipelineResponse = {
			exit_code: 0,
			instructions: []
		};

		for (const script of call.request.scripts) {
			let instruction: RunPipelineInstruction = {
				script,
				has_error: false
			};

			try {
				const options: ExecSyncOptionsWithStringEncoding = { encoding: "utf-8", stdio: "pipe", cwd: `app/scripts/` };
				var stdout: string = execSync(`node ${script}.js`, options);

				instruction.stdout = stdout;
			} catch (error: any) {
				instruction.has_error = true;
				instruction.stderr = error.stderr ? error.stderr.toString() : undefined;
				instruction.stdout = error.stdout ? error.stdout.toString() : undefined;
				response.exit_code = error.status || 1;
			}

			response.instructions?.push(instruction);

			if (response.exit_code !== 0) {
				break;
			}
		}

		callback(null, response);
	}

	public buildConnector(call: ServerUnaryCall<BuildConnectorRequest__Output, BuildConnectorResponse>, callback: sendUnaryData<BuildConnectorResponse>): void {
		const response: BuildConnectorResponse = {
			exit_code: 0
		};

		try {
			fs.writeFileSync("app/scripts/index.js", call.request.index);
			fs.writeFileSync("app/scripts/package.json", call.request.package);

			const options: ExecSyncOptionsWithStringEncoding = { encoding: "utf-8", stdio: "pipe", cwd: `app/scripts/` };
			execSync("npm install", options);
			execSync("node index.js", options);
			execSync(`ncc build index.js --minify`, options);

			var file: Buffer = fs.readFileSync("app/scripts/dist/index.js");
			response.dist = file;
		} catch (error: any) {
			response.exit_code = Number(error.status) || 1;
			response.stderr = error.stderr ? error.stderr.toString() : undefined;
		}

		callback(null, response);
	}
}
