FROM node:20-alpine3.17 AS builder
WORKDIR /usr/src/houston
COPY . .
RUN npm install
RUN npm run build

FROM node:20-alpine3.17 AS final
WORKDIR /usr/src/houston
COPY --from=builder /usr/src/houston/dist ./app
COPY --from=builder /usr/src/houston/proto ./proto
COPY package*.json .
RUN npm ci --omit=dev && \
	npm i -g @vercel/ncc
RUN apk update && \
	apk add git && \
	apk add openssh

EXPOSE 50051

CMD [ "node", "app/index.js" ]
