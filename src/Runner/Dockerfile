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
RUN mkdir -p /usr/src/houston/app/scripts

EXPOSE 50051

CMD [ "node", "app/index.js" ]

# FROM node:20-alpine3.17

# WORKDIR /usr/src/houston

# COPY package*.json ./

# RUN npm ci --omit=dev

# COPY . .

# RUN yarn install  && \
# 	mkdir -p /usr/src/houston/app/scripts

# RUN npm i -g @vercel/ncc

# EXPOSE 50051

# CMD [ "node", "dist/index.js" ]
