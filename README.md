
# Pension Contribution Management System

A robust and scalable .NET 8 Web API built with **Clean Architecture**, **Entity Framework Core**, and **Hangfire** to manage pension member registration, contribution processing, and benefit eligibility for employers and pension fund administrators.

## 📌 Features

### 🧍 Member Management
- Register, update, retrieve, and soft-delete members.
- Age restriction validation (18–70 years).
- Linked to employers for traceability.

### 💸 Contribution Processing
- Handle monthly and voluntary contributions.
- Enforce one monthly contribution per month.
- Interest calculation on annual basis.
- Business rule enforcement: 1-year minimum before benefit eligibility.

### ⏳ Background Jobs (via Hangfire)
- Validate contributions (e.g., duplicates, data completeness).
- Recalculate interest and eligibility.
- Retry failed transactions.
- Notify members/admin of key changes.

### 🛡️ Authentication & Authorization
- JWT-based login for employers.
- Secure endpoints with Bearer token authentication.

## 🧱 Architecture Overview

This system follows **Clean Architecture + Domain-Driven Design**:

```
PensionSystem (Solution)
│
├── API                 → Presentation layer (controllers, middlewares)
├── Application         → Interfaces, DTOs, and business contracts
├── Domain              → Core domain models and enums
├── Infrastructure      → EF Core setup, services, repository impl., identity
```

✅ **SOLID principles**  
✅ **Repository + UnitOfWork pattern**  
✅ **Global exception handling**  
✅ **Separation of concerns**

## 🚀 Tech Stack

- [.NET 8](https://dotnet.microsoft.com)
- **Entity Framework Core**
- **SQL Server**
- **Hangfire**
- **JWT Authentication**
- **Swagger/OpenAPI**
- **xUnit + Moq** for unit testing

## 🔧 Getting Started

### 📁 Clone the Repo

```bash
git clone https://github.com/your-username/your-repo-name.git
cd PensionSystem
```

### ⚙️ Setup the Database

Ensure you have **SQL Server** running locally or remotely.

Update your connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\MSSQLLocalDB;Database=PensionDb;Trusted_Connection=True;"
  }
}
```

### 📦 Apply Migrations

Run from the solution root:

```bash
dotnet ef database update --project Infrastructure --startup-project API
```

This applies all EF Core migrations and sets up the `PensionDb`.

### ▶️ Run the App

```bash
cd API
dotnet run
```

Browse the API at: [https://localhost:7065/swagger](https://localhost:7065/swagger)

## 🔑 Authentication (JWT)

After registration, employers receive a JWT token via `/api/Auth/login`.

Use this token in Swagger:

1. Click **Authorize**.
2. Enter `Bearer {your_token}`.
3. Make authenticated requests to member and contribution endpoints.

## 🧪 Unit Testing

Tests are located in the `Tests` folder and cover services, validation, and repository logic.

Run tests:

```bash
dotnet test
```

Minimum coverage: **70%**

## 🔁 Background Jobs with Hangfire

Hangfire dashboard available at `/hangfire`

Recurring Jobs:
- **ContributionValidationJob** (daily)
- **EligibilityUpdateJob** (daily)
- **FailedTransactionJob** (daily)

Startup registration is in `Program.cs`:

```csharp
recurringJobs.AddOrUpdate<ContributionValidationJob>(
  "ContributionValidationJob",
  job => job.ExecuteAsync(),
  Cron.Daily);
```

## 📂 API Endpoints Summary

### Employer Auth

| Method | Endpoint              | Description         |
|--------|-----------------------|---------------------|
| POST   | `/api/Auth/register`  | Register employer   |
| POST   | `/api/Auth/login`     | Authenticate employer|

### Members

| Method | Endpoint                | Description              |
|--------|-------------------------|--------------------------|
| GET    | `/api/members`          | Get all members          |
| POST   | `/api/members`          | Register new member      |
| PUT    | `/api/members/{id}`     | Update member            |
| DELETE | `/api/members/{id}`     | Soft-delete member       |

### Contributions

| Method | Endpoint                     | Description                       |
|--------|------------------------------|-----------------------------------|
| POST   | `/api/contributions/monthly` | Add monthly contribution          |
| POST   | `/api/contributions/voluntary` | Add voluntary contribution        |
| GET    | `/api/contributions/{memberId}/summary` | View total & interest |

## 📜 Migrations

Migration scripts auto-generated with:

```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
```

## 📄 Design Decisions

| Area              | Choice                                 |
|-------------------|----------------------------------------|
| Architecture      | Clean Architecture + DDD               |
| Auth              | JWT Bearer token                       |
| ORM               | EF Core + Code First                   |
| Background Jobs   | Hangfire + SqlServer Storage           |
| Repository Pattern| Generic + Unit of Work abstraction     |
| Validation        | FluentValidation-style service logic   |
| Error Handling    | Global middleware                      |
| DB Setup          | Code-first, uses `ApplicationDbContext`|

## 🛡️ Security

- JWT token required for member-related endpoints.
- No password-based login (simplified for demo).
- Hangfire dashboard is public in development only.

