import { ServerCredentials } from "@grpc/grpc-js";
import getServer from "./server";

const SERVER_HOST = `0.0.0.0:${process.env.PORT || 50051}`;

const server = getServer();

server.bindAsync(
	SERVER_HOST,
	ServerCredentials.createInsecure(),
	(err: Error | null, port: number) => {
		if (err) {
			console.error(`Server error: ${err.message}`);
		} else {
			console.log(`Server bound on port: ${port}`);

			server.start();
		}
	}
);
