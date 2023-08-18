FROM node:20-alpine3.17

WORKDIR /usr/src/houston

COPY package*.json ./

RUN npm ci --omit=dev

COPY . .

RUN mkdir -p /usr/src/houston/app/scripts

RUN npm i -g @vercel/ncc

EXPOSE 50051

CMD [ "node", "app/index.js" ]
