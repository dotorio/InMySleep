FROM node:20-alpine AS build

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npx tsc

FROM node:20-alpine

WORKDIR /app

COPY package*.json ./
RUN npm install --only=production

COPY --from=build /app/dist ./dist

CMD ["node", "dist/server.js"]