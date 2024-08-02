#!/bin/bash

# Wait for the database to be ready
echo "Waiting for database..."
sleep 10

# Run migrations
echo "Running migrations..."
dotnet ef database update --project src/HaftcinChallenge.Infrastructure --startup-project src/HaftcinChallenge.Api

# Start the application
echo "Starting the application..."
dotnet HaftcinChallenge.Api.dll
