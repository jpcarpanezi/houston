import simpleGit from "simple-git";

export default class Git {
	clone(call: any, callback: any) {
		let response = {
			hasError: false,
		}

		simpleGit().clone(call.request.url, { "--branch": call.request.branch });

		callback(null, response);
	}
}
