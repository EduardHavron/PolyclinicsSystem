FROM node:14
RUN apt-get update
RUN apt-get -y install openssl
RUN apt-get -y install bzip2

WORKDIR /app/http

ENV PATH /app/http/node_modules/.bin:$PATH

COPY package.json /app/http/package.json

RUN npm install
RUN npm install @angular/cli -g

CMD ["npm", "run", "start-docker"]
