FROM node:20-alpine

WORKDIR /app

COPY node_modules ./node_modules

COPY dist ./dist

COPY src/*.pem ./dist

CMD ["node", "dist/server.js"]