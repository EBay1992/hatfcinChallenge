# 🎉 Haftcin Challenge Project

## 📖 Project Overview

The Haftcin Challenge project is a robust application built using .NET 8, designed to demonstrate the integration of multiple modern software development practices and patterns. While not all of these elements may be necessary for every use case, this project serves as an exemplary showcase of how they can be effectively combined to create a well-structured application.

## 🚀 Features

- **🔒 JWT Authentication**: Protects private routes by implementing JSON Web Tokens (JWT), ensuring that only authorized users can access sensitive endpoints.

- **🚦 Rate Limiting**: Controls the number of requests a user can make to the API, ensuring fair usage and preventing abuse.

- **✅ Validation with Fluent Validation**: Implements pipeline behavior for seamless validation of requests, ensuring data integrity and compliance with business rules.

- **📚 CQRS (Command Query Responsibility Segregation)**: Separates read and write operations to enhance performance and scalability.

- **📬 Mediator Pattern**: Facilitates communication between different parts of the application without tight coupling, promoting a cleaner architecture.

- **🏗️ Clean Architecture**: Ensures that the application is organized, maintainable, and testable by following clean architecture principles.

- **🧪 Unit Testing**: Focuses on the domain layer to ensure that the application remains in a valid state and prevents erroneous conditions through comprehensive testing.

- **🔄 Integration Testing**: Validates the interaction between different components and external systems, ensuring expected behavior in real-world scenarios.

- **📝 Logging with Serilog**: Provides structured logging capabilities for easier monitoring and troubleshooting.

- **🌱 Seed Data**: Automatically populates the database with initial data, facilitating development and testing.

- **⏳ OTP Expiration**: Implements expiration for One-Time Passwords (OTP) to enhance security and ensure that verification attempts are timely.

- **🔐 OTP Hashing**: Hashes OTPs before storing them in the database to keep them safe, ensuring that even if the database is compromised, the actual OTPs remain secure.

- **🗄️ EF Core with MSSQL Server**: Utilizes Entity Framework Core (EF Core) for data access, integrated with Microsoft SQL Server to provide a powerful and efficient database management solution.

## 🛣️ API Routes

The Haftcin Challenge API provides several endpoints for user authentication and management:

### Authentication Routes

- **POST** `/api/auth/register`

  - **Description**: Register a new user.

- **POST** `/api/auth/login`

  - **Description**: Log in an existing user.

- **POST** `/api/auth/{id}/verify-otp`

  - **Description**: Verify the OTP for a user.

  - **Path Parameter**: `id` (UUID of the user)

### User Management Routes

- **PUT** `/api/users/{id}/complete-profile`

  - **Description**: Complete the profile of a user.
  - **Path Parameter**: `id` (UUID of the user)

- **GET** `/api/users`
  - **Description**: Retrieve a list of users with optional search and pagination.
  - **Query Parameters**:
    - `searchTerm` (string): Optional search term.
    - `page` (integer): Page number (default: 1).
    - `pageSize` (integer): Number of users per page (default: 10).

## 📅 Todo

1. 📈 Implement performance monitoring tools to identify bottlenecks and optimize application efficiency.

2. 🧪 Expand the test suite to cover more scenarios, improving overall code reliability and catching edge cases.

3. ✅ Enhance request validation to enforce stricter business rules and prevent processing of invalid data.

4. 🔄 Introduce caching mechanisms to reduce database load and improve response times for frequently accessed data.

5. 🐳 Refine Docker implementation to ensure optimal containerization and efficient deployment across environments.

---

## 🛠️ How to Use

Follow these steps to get started with the Haftcin Challenge project:

# 🛠️ How to Use Haftcin Challenge Project

Follow these steps to get started with the Haftcin Challenge project:

### 1. Clone the Repository

First, clone the repository to your local machine using Git. Open your terminal and run:

```bash
git clone https://github.com/EBay1992/hatfcinChallenge.git
```

### 2. Navigate to the Project Directory

Change into the project directory:

```bash
cd hatfcinChallenge
```

### 3. Adjust Database Connection

Open the `appsettings.json` file located in the `src/HaftcinChallenge.Api` directory. Update the connection string to match your local SQL Server setup. Look for the `ConnectionStrings` section and modify it as follows:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
}
```

Replace:

- `your_server` with the name or IP address of your SQL Server instance.
- `your_database` with the name of the database you want to use.
- `your_user` with your SQL Server username.
- `your_password` with your SQL Server password.

### 4. Build the Project

To build the project, navigate to the `src` directory and run the following command:

```bash
 dotnet build src/HaftcinChallenge.Api/HaftcinChallenge.Api.csproj
```

Certainly! I'll add a note about updating the global EF tool before running migrations. Here's the updated section:

### 5. Run Migrations

Before running the application, you need to apply any pending migrations to set up the database schema. However, first ensure that you have the latest version of the Entity Framework Core tools installed globally. You can update the tools using the following command:

```bash
dotnet tool update --global dotnet-ef
```

If you haven't installed the EF Core tools globally before, you can install them using:

```bash
dotnet tool install --global dotnet-ef
```

Once you have the latest EF Core tools, you can apply the migrations using the following command:

```bash
dotnet ef database update -p src/HaftcinChallenge.Infrastructure/HaftcinChallenge.Infrastructure.csproj -s src/HaftcinChallenge.Api/HaftcinChallenge.Api.csproj
```

This command will apply all pending migrations to your database, creating or updating the necessary tables and schema.

### 6. Start the Application

You can now start the application by specifying the startup project `src/HaftcinChallenge.Api` directory and running:

```bash
dotnet run --project src/HaftcinChallenge.Api/HaftcinChallenge.Api.csproj
```

### 7. Access the API

Once the application is running, you can access the API at `http://localhost:5015/swagger/index.html`.

### 8. Run Unit Tests

To run unit tests, specifying the tests/HaftcinChallenge.Domain.Tests directory and execute the following command:

```bash
 dotnet test tests/HaftcinChallenge.Domain.Tests/HaftcinChallenge.Domain.Tests.csproj
```

Certainly. I'll add a note about the potential brittleness of the tests. Here's an updated version of the section:

### 9. Run Integration Tests

To run integration tests, navigate to the `tests/HaftcinChallenge.IntegrationTests` directory and execute the following command:

```bash
dotnet test tests/HaftcinChallenge.IntegrationTests/HaftcinChallenge.IntegrationTests.csproj
```

**Important Note:**
The current integration tests have some inherent brittleness and may not run successfully 100% of the time. This is a known issue that should be addressed in future improvements. Some potential reasons for this instability could include:

1. Timing issues with asynchronous operations
2. Potential race conditions in test setup or teardown
3. Dependency on external services or databases that may have occasional hiccups
4. Insufficient isolation between test cases

When running these tests, you may occasionally encounter failures that are not indicative of actual problems in the code, but rather issues with the test environment or setup. If you encounter consistent failures, it's worth investigating further, but occasional failures may be due to the current brittleness of the test suite.
