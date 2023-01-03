FROM node:19.2-alpine as build

ENV NODE_ENV=production
COPY . .
RUN npm install --production
RUN npm run build
COPY . .

FROM nginx:stable-alpine
COPY --from=build /build /usr/share/nginx/html
CMD ["nginx", "-g", "daemon off;"]