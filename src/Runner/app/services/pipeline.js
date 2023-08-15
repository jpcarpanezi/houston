import { execSync } from "child_process";

export default class Pipeline {
	constructor() { }

	runPipeline(call, callback) {
		let response = {
			exitCode: 0,
			instructions: []
		};

		for (const script of call.request.scripts) {
			let instruction = {
				script,
				hasError: false,
				stdout: null,
				stderr: null
			};

			try {
				const options = { encoding: "utf-8", stdio: "pipe", cwd: `app/scripts/${script}/` };
				execSync("npm install", options);
				var stdout = execSync(`node ${script}.js`, options);

				instruction.stdout = stdout;
			} catch (error) {
				instruction.hasError = true;
				instruction.stderr = error.stderr ? error.stderr.toString() : null;
				instruction.stdout = error.stdout ? error.stdout.toString() : null;
				response.exitCode = error.status || 1;
			}

			response.instructions.push(instruction);

			if (response.exitCode !== 0) {
				break;
			}
		}

		callback(null, response);
	}
}
