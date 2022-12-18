FROM node:19.2-alpine as build

WORKDIR /src
COPY package-lock.json .
COPY package.json .
RUN npm install --silent
RUN npm build
COPY . .
CMD ["serve", "-s", "build", "-l", "4567"]
