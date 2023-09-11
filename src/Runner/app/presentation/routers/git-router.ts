import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import { CloneRepositoryRequest__Output } from "../../proto/houston/v1/git/CloneRepositoryRequest";
import { CloneRepositoryResponse } from "../../proto/houston/v1/git/CloneRepositoryResponse";
import { GitServiceHandlers } from "../../proto/houston/v1/git/GitService";
import GitService from "../../services/git";

const gitService = new GitService();

export const GitRouter: GitServiceHandlers = {
	CloneRepository: function (call: ServerUnaryCall<CloneRepositoryRequest__Output, CloneRepositoryResponse>, callback: sendUnaryData<CloneRepositoryResponse>): void {
		gitService.clone(call, callback);
	}
}
