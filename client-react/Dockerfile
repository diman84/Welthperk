FROM node:boron
COPY . /app
WORKDIR /app
RUN ["npm", "install"]
EXPOSE 3000/tcp
ENV NODE_PATH ./src
ENV APIPORT 80
ENV PORT 3000
ENV NODE_ENV production
ENTRYPOINT ["node", "./bin/server.js"]
