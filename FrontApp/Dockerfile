FROM node:19.2-alpine as build

WORKDIR /src
COPY public .
COPY package-lock.json .
COPY package.json .
RUN npm install --silent
RUN npm run build
COPY . .
CMD ["serve", "-s", "build", "-l", "4567"]
