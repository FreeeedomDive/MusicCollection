FROM node:19.2-alpine as build

ENV NODE_ENV=production
COPY . .
RUN npm install --production
RUN npm run build
RUN npm install -g serve
COPY . .
CMD ["serve", "-s", "build", "-l", "4567"]
