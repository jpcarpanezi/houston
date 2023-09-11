import grpc from "@grpc/grpc-js";
import protoLoader from "@grpc/proto-loader";
import Pipeline from "./services/pipeline.js";
import "dotenv/config";

var PROTO_PATH = "app/protos/server.proto";

var packageDefinition = protoLoader.loadSync(
	PROTO_PATH,
	{
		keepCase: true,
		longs: String,
		enums: String,
		defaults: true,
		oneofs: true
	}
);
var proto_server = grpc.loadPackageDefinition(packageDefinition).houston;

function main() {
	var server = new grpc.Server();
	var pipeline = new Pipeline();
	server.addService(proto_server.Houston.service, { runPipeline: pipeline.runPipeline });
	server.bindAsync(`0.0.0.0:${process.env.SERVER_PORT}`, grpc.ServerCredentials.createInsecure(), () => {
		server.start();
	});
}

main();
