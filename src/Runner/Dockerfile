FROM node:18.15.0

WORKDIR /usr/src/houston

COPY package*.json ./

RUN npm install

COPY . .

EXPOSE 50051
CMD [ "node", "app/index.js" ]
