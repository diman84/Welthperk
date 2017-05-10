FROM node:boron
COPY . /app
WORKDIR /app
COPY package.json /app
RUN ["npm", "install"]
EXPOSE 3000/tcp
ENV NODE_PATH ./scr
ENV APIPORT 5000
ENV PORT 3000
ENV NODE_ENV production
ENTRYPOINT ["node", "./bin/server.js"]
