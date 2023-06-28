# Use an official Node.js runtime as the base image
FROM node:18

# Set the working directory in the container to /app
WORKDIR /web

# Copy the package.json and package-lock.json files to the container
COPY package*.json ./

# Install the app's dependencies in the container
RUN npm install

# Copy the rest of the app's files to the container
COPY . .

# Build the app
RUN npm run build

# Specify the command to run when the container starts
CMD ["npm", "start"]
