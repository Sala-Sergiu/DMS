# DMS — Device Management System

A full-stack enterprise-style application for managing company-owned devices, tracking technical specifications, and monitoring device assignments.

Stack: ASP.NET Core Web API (.NET 8) · Entity Framework Core · MS SQL Server · Angular 17 · JWT Authentication · OpenAI

## Prerequisites

Make sure you have the following installed:

| Tool | Version |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download) | 8.0 or later |
| [Node.js](https://nodejs.org/) | 18.0 or later |
| [Angular CLI](https://angular.io/cli) | 17.0 or later |
| [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) | 2019 or later (or LocalDB) |
| [SSMS](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) | Any recent version |

## 1. Database Setup

Open SSMS and run the following scripts in order:
-- Step 1: Create tables database/01_create_tables.sql
-- Step 2: Seed dummy data database/02_seed_data.sql

Both scripts are idempotent, safe to run multiple times.

Default connection string (targets `localhost`):

Server=localhost;Database=DMS;Trusted_Connection=True;TrustServerCertificate=True;

If your SQL Server instance has a different name, update it in:
src/DMS.API/appsettings.json → ConnectionStrings.DefaultConnection

## 2. Backend Setup

The API will start at: **https://localhost:50010
Swagger UI is available at: **https://localhost:5000/swagger
### OpenAI API Key (required for AI description generation)
Add your key in `src/DMS.API/appsettings.json`:
"OpenAi": { "ApiKey": "your-openai-api-key-here", "Model": "gpt-4o-mini" }

If left empty, the Generate Description feature will return an error but all other features work normally.


## 3. Frontend Setup

cd ui/DMS.Web npm install ng serve

The Angular app will start at: http://localhost:4200
The Angular dev server is configured to proxy `/api` requests to `https://localhost:5000` automatically.

## Features

- ✅ Full CRUD for devices with validation
- ✅ JWT authentication with BCrypt password hashing
- ✅ Device assignment and unassignment per user
- ✅ Relevance-ranked free-text search (no AI)
- ✅ AI-generated device descriptions via OpenAI
- ✅ Pagination, filtering and sorting
- ✅ Correlation ID tracing on all requests
- ✅ Global exception handling middleware
- ✅ Structured logging with Serilog
- ✅ Unit tests + integration tests
