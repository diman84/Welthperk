FROM node:boron
COPY . /app
WORKDIR /app
RUN ["npm", "install"]
RUN ["npm", "run", "postinstall"]
EXPOSE 3000/tcp
ENV NODE_PATH ./src
ENV PORT 3000
ENV NODE_ENV production
ENV WEBPACK_DLLS 1
ENTRYPOINT ["node", "./bin/server.js"]
