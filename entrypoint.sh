#!/bin/bash

# Wait for the database to be ready
echo "Waiting for SQL Server to be available..."
while ! nc -z db 1433; do
  sleep 1
done
echo "SQL Server is up - executing command"

# Run migrations
dotnet ef database update --project HaftcinChallenge.Infrastructure/HaftcinChallenge.Infrastructure.csproj

# Start the application
exec dotnet HaftcinChallenge.Api.dll