# Stage 0, for downloading project’s npm dependencies, building and compiling the app.
FROM node:latest as node

# set working directory
RUN mkdir /usr/src/app
WORKDIR /usr/src/app


# install package.json (o sea las dependencies)
COPY package*.json ./
RUN npm install -g @angular/cli @angular-devkit/build-angular && npm install

EXPOSE 4201

CMD ["npm", "start"]
