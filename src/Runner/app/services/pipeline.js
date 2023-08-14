import { execSync } from "child_process";

export default class Pipeline {
	constructor() { }

	runPipeline(call, callback) {
		let response = {
			exitCode: 0,
			stdout: null,
			stderr: null,
			function_with_error: null
		};

		for (const script of call.request.scripts) {
			try {
				console.log(`Running script: ${script}`);

				var stdout = execSync(`node app/scripts/${script}.js`);

				if (stdout != null)
					response.stdout = stdout.toString();
			} catch (error) {
				response.exitCode = error.status;
				response.stderr = error.stderr;
				response.stdout = error.stdout;
				response.function_with_error = script;
			}
		}

		callback(null, response);
	}
}
