import { ServerUnaryCall, sendUnaryData } from "@grpc/grpc-js";
import { CloneRepositoryRequest__Output } from "../../proto/houston/v1/git/CloneRepositoryRequest";
import { CloneRepositoryResponse } from "../../proto/houston/v1/git/CloneRepositoryResponse";
import { GitServiceHandlers } from "../../proto/houston/v1/git/GitService";

export const GitRouter: GitServiceHandlers = {
	CloneRepository: function (call: ServerUnaryCall<CloneRepositoryRequest__Output, CloneRepositoryResponse>, callback: sendUnaryData<CloneRepositoryResponse>): void {
		throw new Error("Function not implemented.");
	}
}
