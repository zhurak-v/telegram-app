FROM node:18.17.0-alpine

RUN apk update

WORKDIR /client

COPY package*.json ./

RUN npm install --force

COPY . .

ENV NODE_ENV Production

RUN npm cache clean --force

RUN npm run build

CMD ["npm", "run", "start"]

EXPOSE 3000
