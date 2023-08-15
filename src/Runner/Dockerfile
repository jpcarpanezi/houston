FROM node:18.15.0

WORKDIR /usr/src/houston

COPY package*.json ./

RUN npm ci --omit=dev

COPY . .

RUN mkdir -p /usr/src/houston/app/scripts

EXPOSE 50051
CMD [ "node", "app/index.js" ]
