#!/bin/bash

# Navigate to the project directory
cd "E:/Back End/1- Dating Application/Server/Dating/api"

# Restore the dependencies
dotnet restore

# Build the project
dotnet build --configuration Release

# Run the project
dotnet run --urls "https://localhost:5001"