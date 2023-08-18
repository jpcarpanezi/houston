import { Server, loadPackageDefinition } from "@grpc/grpc-js";
import { Options, loadSync } from "@grpc/proto-loader";
import { PipelineRouter } from "./presentation/routers/pipeline-router";
import { GitRouter } from "./presentation/routers/git-router";
import { ProtoGrpcType } from "./proto/proto";

export default function getServer(): Server {
	const server = new Server();

	const filename: string[] = [
		"proto/houston/v1/pipeline.proto",
		"proto/houston/v1/git.proto",
	];

	const options: Options = {
		keepCase: true,
		longs: String,
		enums: String,
		defaults: true,
		oneofs: true
	}

	const packageDefinition = loadSync(filename, options);

	const proto = loadPackageDefinition(packageDefinition) as unknown as ProtoGrpcType;

	server.addService(proto.houston.v1.pipeline.PipelineService.service, PipelineRouter);
	server.addService(proto.houston.v1.git.GitService.service, GitRouter);

	return server;
}
