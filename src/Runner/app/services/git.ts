import simpleGit, { GitResponseError, Response, SimpleGitOptions } from "simple-git";
import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import { CloneRepositoryRequest__Output } from "../proto/houston/v1/git/CloneRepositoryRequest";
import { CloneRepositoryResponse } from "../proto/houston/v1/git/CloneRepositoryResponse";
import fs from "fs";
import { execSync } from "child_process";

const OUTPUT_FOLDER = process.env.SCRIPTS_FOLDER || "app/scripts";
const SSH_FOLDER = "/root/.ssh/";

export default class GitService {
	public async clone(call: ServerUnaryCall<CloneRepositoryRequest__Output, CloneRepositoryResponse>, callback: sendUnaryData<CloneRepositoryResponse>) {
		let response: CloneRepositoryResponse = {
			has_error: false,
		}

		try {
			if (!fs.existsSync(OUTPUT_FOLDER)) {
				fs.mkdirSync(OUTPUT_FOLDER);
			}

			if (!fs.existsSync(SSH_FOLDER)) {
				fs.mkdirSync(SSH_FOLDER);
			}

			fs.writeFileSync("/root/.ssh/id_rsa", Buffer.from(call.request.private_key, "base64").toString("utf-8"));
			fs.chmodSync("/root/.ssh/id_rsa", 0o400);
			execSync("ssh-keyscan -H github.com >> /root/.ssh/known_hosts");

			const options: Partial<SimpleGitOptions> = { baseDir: "app/scripts" };
			await simpleGit(options).clone(call.request.url, ".", { "--branch": call.request.branch });
		} catch (error) {
			const err = error as GitResponseError;
			response.has_error = true;
			response.stderr = err.message ? err.message.toString() : undefined;
		}

		callback(null, response);
	}
}
