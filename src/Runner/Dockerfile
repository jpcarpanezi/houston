FROM node:20 AS builder
WORKDIR /usr/src/houston
COPY . .
RUN npm install
RUN npm run build

FROM node:20 AS final
WORKDIR /usr/src/houston
COPY --from=builder /usr/src/houston/dist ./app
COPY --from=builder /usr/src/houston/proto ./proto
COPY package*.json .
RUN npm ci --omit=dev && \
	npm i -g @vercel/ncc
RUN apt-get update && \
	apt-get install -y git && \
	apt-get install -y openssh-server openssh-client

EXPOSE 50051

CMD [ "node", "app/index.js" ]
