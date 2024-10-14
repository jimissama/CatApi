# The .NET image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory
WORKDIR /app

# Install Git
RUN apt-get update && apt-get install -y git

# Clone the CatAPI project from GitHub
RUN git clone https://github.com/jimissama/CatApi.git

# Change directory to the cloned project
WORKDIR /app/CatApi/CatApi

# DB Connection string
ENV CON_STR="YOUR DB CONNECTION STRING"

# The cat api key
ENV CAT_API_KEY="YOUR CAT API KEY"

# Update appsettings.json with db connection string and cat api key
RUN sed -i "s/CONSTR/${CON_STR}/" appsettings.json
RUN sed -i "s/CATAPIKEY/${CAT_API_KEY}/" appsettings.json

# Install .NET dependencies
RUN dotnet restore

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef --version 8.*

# Insert tools path
ENV PATH="$PATH:/root/.dotnet/tools"

# Run database migrations
RUN dotnet ef migrations add Initial

# Run database migrations
RUN dotnet ef database update

# Build the application
RUN dotnet publish -c Release -o /app/out

# Expose 8080 port
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "/app/out/CatApi.dll"]
