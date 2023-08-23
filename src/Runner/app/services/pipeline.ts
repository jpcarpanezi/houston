import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import { ExecSyncOptionsWithStringEncoding, execSync } from "child_process";
import { RunPipelineRequest__Output } from "../proto/houston/v1/pipeline/RunPipelineRequest";
import { RunPipelineResponse } from "../proto/houston/v1/pipeline/RunPipelineResponse";
import { RunPipelineInstruction } from "../proto/houston/v1/pipeline/RunPipelineInstruction";
import { BuildConnectorFunctionRequest__Output } from "../proto/houston/v1/pipeline/BuildConnectorFunctionRequest";
import { BuildConnectorFunctionResponse } from "../proto/houston/v1/pipeline/BuildConnectorFunctionResponse";
import tar from "tar";
import fs from "fs";
import { promisify } from "util";
import { randomUUID } from "crypto";

const OUTPUT_FOLDER = process.env.SCRIPTS_FOLDER || "app/scripts";

export default class PipelineService {
	public async runPipeline(call: ServerUnaryCall<RunPipelineRequest__Output, RunPipelineResponse>, callback: sendUnaryData<RunPipelineResponse>): Promise<void> {
		let response: RunPipelineResponse = {
			exit_code: 0,
			instructions: []
		};

		const scriptId: string = randomUUID();

		try {
			if (!fs.existsSync(OUTPUT_FOLDER)) {
				fs.mkdirSync(OUTPUT_FOLDER);
			}

			const outputTar = `${OUTPUT_FOLDER}/${scriptId}/${scriptId}.tar.gz`;
			const outputTarFolder = `${OUTPUT_FOLDER}/${scriptId}`;

			fs.mkdirSync(outputTarFolder, { recursive: true });
			fs.writeFileSync(outputTar, call.request.files);
			const extractOptions: tar.ExtractOptions & tar.FileOptions = { file: outputTar, cwd: outputTarFolder };
			await promisify(tar.x)(extractOptions, undefined);
		} catch (error: any) {
			response.exit_code = error.status || 1;
		}

		if (response.exit_code != 0) {
			callback(null, response);
			return;
		}

		for (const script of call.request.scripts) {
			let instruction: RunPipelineInstruction = {
				script,
				has_error: false
			};

			try {
				const options: ExecSyncOptionsWithStringEncoding = { encoding: "utf-8", stdio: "pipe", cwd: `app/scripts/` };
				var stdout: string = execSync(`node ${scriptId}/${script}`, options);

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

	public async buildConnector(call: ServerUnaryCall<BuildConnectorFunctionRequest__Output, BuildConnectorFunctionResponse>, callback: sendUnaryData<BuildConnectorFunctionResponse>): Promise<void> {
		const response: BuildConnectorFunctionResponse = {
			exit_code: 0
		};

		try {
			if (!fs.existsSync(OUTPUT_FOLDER)) {
				fs.mkdirSync(OUTPUT_FOLDER);
			}

			const outputTar = `${OUTPUT_FOLDER}/index.tar.gz`;

			fs.writeFileSync(outputTar, call.request.files);
			const extractOptions: tar.ExtractOptions & tar.FileOptions = { file: outputTar, cwd: OUTPUT_FOLDER };
			await promisify(tar.x)(extractOptions, undefined);

			const options: ExecSyncOptionsWithStringEncoding = { encoding: "utf-8", stdio: "pipe", cwd: OUTPUT_FOLDER };
			execSync("npm install", options);
			execSync("ncc build index.js --minify", options);

			var file: Buffer = fs.readFileSync(`${OUTPUT_FOLDER}/dist/index.js`);
			response.dist = file;
			response.type = this.getPackageType(OUTPUT_FOLDER) || "commonjs";
		} catch (error: any) {
			response.exit_code = Number(error.status) || 1;
			response.stderr = error.stderr ? error.stderr.toString() : undefined;
		}

		callback(null, response);
	}

	private getPackageType(outputFolder: string): string | undefined {
		const packageJson = fs.readFileSync(`${outputFolder}/package.json`);
		const packageObject: PackageJson = JSON.parse(packageJson.toString());

		return packageObject.type;
	}
}
