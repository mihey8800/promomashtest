# Use the official Node.js image as the base image
FROM node:22 AS build

# Set the working directory
WORKDIR /usr/src/app

COPY ui/ .

# Install dependencies
RUN npm install



# Install Angular CLI globally
RUN npm install -g @angular/cli

# Expose the port the app runs on
EXPOSE 4200

# Start the application
CMD ["ng", "serve", "--host", "0.0.0.0"]