# DMS Copilot Instructions

## Project Overview

This repository contains **DMS (DeviceManagementSystem)**, a layered enterprise-style application built for a technical assignment.

The goal of the application is to manage company-owned mobile devices and track:
- device technical details
- current user assignment
- user information
- authentication and authorization
- AI-generated device descriptions
- search, filtering, sorting, and pagination

The solution must look like a real business application, not a demo-only CRUD app.

The application stack is:

- Backend: C#, ASP.NET Core Web API, .NET 8
- Database: SQL Server
- Frontend: Angular (implemented later, not part of the current backend-first phase)
- Version Control: Git
- AI integration: LLM-based device description generator

The solution structure must remain simple, consistent, and maintainable.

---

## High-Level Architectural Rules

This project uses a strict layered architecture:

- `DMS.API`
- `DMS.BLL`
- `DMS.DAL`
- `DMS.Domain`

### Dependency Rules

Allowed project references:

- `DMS.API` -> `DMS.BLL`
- `DMS.BLL` -> `DMS.DAL`, `DMS.Domain`
- `DMS.DAL` -> `DMS.Domain`
- `DMS.Domain` -> no project references

These dependency rules must not be violated.

### Layer Responsibilities

#### DMS.Domain
Contains pure domain concepts only:
- entities
- enums
- constants
- domain exceptions
- base classes for entities

No EF Core, no ASP.NET Core, no HTTP concerns, no infrastructure details.

#### DMS.DAL
Contains persistence concerns:
- EF Core DbContext
- entity configurations
- repositories
- generic repository
- specialized repositories
- unit of work
- query composition for search/filter/sort/pagination
- SQL-related persistence behavior

No controller logic and no HTTP-specific logic.

#### DMS.BLL
Contains business logic:
- services
- DTOs
- validation
- business rules
- authorization checks at service level when needed
- auth logic
- AI generation orchestration
- mapping logic
- application exceptions

Services must coordinate workflows and business rules.

#### DMS.API
Contains delivery concerns:
- controllers
- middleware
- dependency injection bootstrapping
- Swagger/OpenAPI config
- authentication and authorization wiring
- correlation id middleware
- logging middleware / request logging
- response headers
- global exception handling middleware

Controllers must be thin.

---

## Solution Structure

The repository structure must remain consistent with this layout:

```text
DMS/
  DMS.sln
  src/
    DMS.API/
    DMS.BLL/
    DMS.DAL/
    DMS.Domain/
  tests/
    DMS.BLL.Tests/
    DMS.API.IntegrationTests/
  database/
    01_create_tables.sql
    02_seed_data.sql

    Do not introduce unnecessary new top-level projects unless explicitly requested.

Do not introduce an additional Infrastructure project unless explicitly requested. Keep the solution within the four main layers.

Core Domain Model

At minimum, the domain should support:

Device

A device represents a company-owned mobile device.

Typical fields:

Id
Name
Manufacturer
Type
OperatingSystem
OperatingSystemVersion
Processor
RamAmount
Description
CurrentUserId (nullable if unassigned)
CreatedAtUtc
UpdatedAtUtc
User

A user represents a system user and potentially the current assignee of a device.

Typical fields:

Id
Name
Email
PasswordHash
Role
Location
CreatedAtUtc
UpdatedAtUtc
Notes on Device Assignment

For this assignment, device assignment can be modeled in one of two ways:

Simple current assignment:
Device.CurrentUserId
easiest and sufficient for the assignment
Separate assignment entity/history:
more complex
only add if explicitly requested

Default to the simpler approach unless instructed otherwise.

Naming Conventions

Use consistent naming throughout the solution.

Projects
DMS.API
DMS.BLL
DMS.DAL
DMS.Domain
Classes
PascalCase
nouns for entities and DTOs
interfaces prefixed with I

Examples:

Device
User
DeviceService
IDeviceService
DeviceRepository
IRepository<TEntity>
IUnitOfWork
Methods

Use clear, intention-revealing names:

GetByIdAsync
GetPagedAsync
CreateAsync
UpdateAsync
DeleteAsync
AssignToCurrentUserAsync
UnassignFromCurrentUserAsync
GenerateDescriptionAsync

Avoid vague names such as:

Handle
DoWork
ProcessItem
RunStuff
DTO Names

Use explicit request/response DTO names:

CreateDeviceRequestDto
UpdateDeviceRequestDto
DeviceResponseDto
LoginRequestDto
LoginResponseDto
GenerateDeviceDescriptionRequestDto
GenerateDeviceDescriptionResponseDto
Controller Design Rules

Controllers must remain thin.

Controllers are responsible for:

receiving HTTP requests
model binding
delegating work to services
returning HTTP responses
setting response headers if needed

Controllers must not:

contain business logic
access DbContext directly
access repositories directly
contain large validation logic
contain data access logic
contain AI prompt-building logic
contain repeated try/catch blocks

Example expectation:

Controller calls service
Service executes business logic
Middleware handles exceptions globally
Repository Pattern and Unit of Work

This project uses:

generic repository
specialized repositories where needed
unit of work
Generic Repository

Use a generic repository for shared persistence operations across entities.

Example responsibilities:

GetByIdAsync
AddAsync
Update
Delete
AnyAsync
FirstOrDefaultAsync

Do not create duplicated CRUD interfaces for every entity if the behavior is generic.

Specialized Repositories

Add specialized repositories only when entity-specific queries are needed.

Examples:

IDeviceRepository
IUserRepository

Typical device-specific repository responsibilities:

get devices with current assignee
get paged devices with search/filter/sort
get available devices
check device assignment state

Typical user-specific repository responsibilities:

get user by email
check email existence
load user by credentials-related lookup
Unit of Work

Use IUnitOfWork to coordinate repository access and persistence.

The unit of work should expose repository properties and a save method.

Typical responsibilities:

Devices
Users
SaveChangesAsync

Repositories must not call SaveChangesAsync internally for every operation. Persistence should be coordinated centrally through the unit of work.

Query Pipeline Pattern

Filtering, sorting, searching, and pagination must be implemented using a pipeline pattern.

This is an explicit project requirement and also a design preference.

Goal

Avoid a single giant method with many nested if statements.

Design

Start with IQueryable<Device> and pass it through separate pipeline steps.

Typical step order:

search
filter
sort
pagination
Expected abstractions

A query pipeline should be built from small, composable steps.

Suggested concepts:

IDeviceQueryStep
DeviceSearchStep
DeviceFilterStep
DeviceSortStep
DevicePaginationStep
Rules
each step should transform IQueryable<Device>
each step must have a single responsibility
the pipeline must remain deterministic
sort logic must be explicit and safe
avoid reflection-heavy magic unless explicitly requested
Search

The bonus requirement expects free-text search across:

Name
Manufacturer
RAM
Processor

Search must:

be case-insensitive
tolerate formatting differences
support token-based matching
rank stronger matches higher than weaker matches

Search must not use AI.

Pagination

Use request parameters like:

pageNumber
pageSize

Add response metadata through:

response body
and/or X-Total-Count response header

For this project, using X-Total-Count is encouraged.

API Design Expectations

Use conventional REST-style endpoints.

Devices

Suggested endpoints:

GET /api/devices
GET /api/devices/{id}
POST /api/devices
PUT /api/devices/{id}
DELETE /api/devices/{id}
POST /api/devices/{id}/assign
POST /api/devices/{id}/unassign
POST /api/devices/generate-description
GET /api/devices/search or use query parameters on GET /api/devices
Auth / Users

Suggested endpoints:

POST /api/auth/register
POST /api/auth/login
GET /api/users/me

Alternative user-focused auth controller naming is allowed if kept consistent.

General Rules
use DTOs, never expose EF entities directly from controllers
return proper status codes
keep route naming consistent
return validation and error responses in a consistent format
Authentication and Authorization

This project requires authentication and authorization.

Authentication

Use JWT bearer authentication.

The login flow should:

validate user credentials
generate JWT token
include appropriate claims
return token to the client

Do not use plain SHA256-only password hashing for passwords.

Preferred approaches:

ASP.NET Core Identity password hasher
PBKDF2
BCrypt
another secure password hashing mechanism

Do not implement insecure password storage.

Authorization

Use role-based and/or policy-based authorization.

Controllers may use:

[Authorize]
[Authorize(Roles = "...")]
[Authorize(Policy = "...")]

Policies and permissions should be centralized in constants where appropriate.

Device Assignment Rule

A user can assign a device to himself only if:

the device exists
the device is not already assigned to another user
the caller is authenticated

A user can unassign a device only if:

the device exists
the device is currently assigned to that same user
the caller is authenticated

These business rules must be enforced in the service layer.

Validation

All important request models must be validated.

Validation must cover:

required fields
logical consistency
uniqueness where appropriate
business rules where appropriate

Examples:

create device: required fields must be present
update device: required fields must be present
register user: email and password required
login user: email and password required
generate description: enough device data must exist to generate useful output

Validation should not be buried inside controllers.

Error Handling

Global exception handling middleware must be implemented.

Requirements
do not scatter repeated try/catch in controllers
catch unhandled exceptions in middleware
map known application exceptions to proper HTTP responses
return a consistent error response shape
log exceptions with correlation id
Suggested exception types
NotFoundException
ConflictException
ValidationException
UnauthorizedException
ForbiddenException
Response shape

Error responses should be predictable and clean.
Avoid returning raw stack traces to the client.

Logging

Logging is required and should look production-minded.

Goals
log requests and responses at a useful level
log errors separately and clearly
include correlation id in logs
keep logs readable
Expectations

Use structured logging if possible.

A logger such as Serilog is encouraged if used cleanly and not overcomplicated.

At minimum, log:

request method
request path
status code
execution time
correlation id
user id if relevant and safe
exception details on failure

Avoid excessive noisy logging.

Correlation ID

A correlation id middleware must be implemented.

Requirements
accept incoming X-Correlation-Id if present
generate one if missing
store it in request context
include it in response headers
include it in logs

This is an important non-functional feature and should be treated as a first-class concern.

Response Headers

The API should support useful response headers where appropriate.

Most notably:

X-Total-Count for paginated list results
X-Correlation-Id for traceability

Do not add headers arbitrarily. Only add headers that provide clear API value.

Database Strategy

The application uses SQL Server.

Database Scripts

The project must include SQL scripts:

01_create_tables.sql
02_seed_data.sql

These scripts must be idempotent.

Idempotency Rule

Running the scripts multiple times must not:

fail because objects already exist
create duplicate data
corrupt state

Examples:

create tables only if they do not already exist
insert seed rows only if they are not already present
EF Core

EF Core may be used for application data access, but the required SQL scripts must still be delivered because the assignment explicitly asks for them.

Do not rely only on migrations without delivering the SQL scripts.

Dummy Data

Seed data is required so the application works immediately.

Using Bogus to help generate realistic dummy data is acceptable, but the final deliverable must still satisfy the requirement for SQL seed scripts.

Suggested approach:

use Bogus during development if helpful
convert final seed set into deterministic SQL script form

The final seed set should be realistic, clean, and small enough to remain understandable.

AI Integration

The project must implement a device description generator.

Goal

Generate a concise, human-readable description of a device based on technical specifications.

Example input fields:

Name
Manufacturer
Type
OperatingSystem
OperatingSystemVersion
Processor
RamAmount

Example output:

a short business-friendly sentence
Design Principles
backend handles AI integration
frontend must not call the LLM provider directly
API keys must never be exposed in Angular
AI logic must be isolated from controllers
use an abstraction for description generation
Recommended Design

Use an application service abstraction such as:

IDeviceDescriptionGenerator

Suggested flow:

controller receives generation request
service validates input
service builds prompt
AI adapter/client sends request to provider
service returns generated description
Prompting

Prompts should be:

concise
deterministic in style
business-oriented
limited to one short description
not overly creative
not marketing-heavy
Error Handling

If the AI provider fails:

log the error
return a clean failure response
do not leak provider internals
do not break unrelated CRUD flows
Scope

Do not implement advanced AI features unless explicitly requested.
Do not add:

embeddings
RAG
chat history
agents
vector databases

This feature is only a focused text-generation use case.

Caching

Caching is optional and should only be added where it clearly improves the application.

If caching is implemented, keep it simple:

use in-memory caching
cache read-heavy endpoints where appropriate
invalidate cache on create/update/delete

Do not introduce distributed caching unless explicitly requested.

Caching must not complicate the project or delay completion of core requirements.

Testing Strategy

Testing is important, but completion of the required functionality is more important than chasing arbitrary percentage targets.

Priorities

Focus testing on:

BLL/service layer
business rules
assignment logic
validation behavior
auth-related service behavior
search/filter/sort pipeline behavior
Unit Tests

Use unit tests with mocking for:

repositories
unit of work
AI generator abstraction
current user abstraction
token generation abstraction where relevant
Integration Tests

If time allows, add a small number of integration tests for:

key API endpoints
authentication flow
global exception handling
Coverage

Do not optimize purely for an 80% number.
Optimize for meaningful tests on critical logic.

Coding Style and Design Rules
General
prefer readability over cleverness
use async methods consistently where appropriate
keep methods focused and short
extract logic into services
avoid duplication
favor explicitness over magic
Controllers
thin
simple
no business logic
Services
enforce business rules
orchestrate repositories
validate domain scenarios
throw meaningful application exceptions
Repositories
persistence-focused
no business policy decisions
compose queries cleanly
Entities
keep entity models clean
do not bloat entities with framework concerns
DTOs
separate request and response DTOs
do not reuse one DTO for everything unless truly appropriate
Mapping

Mapping may be manual or via a mapper library if requested.
If manual mapping is used, keep it centralized and consistent.

Nullability

Use nullable reference types appropriately.
Do not suppress warnings carelessly.

Comments

Prefer good naming over excessive comments.
Add comments only where intent is not obvious.

Non-Functional Expectations

This project should look like a junior-to-strong-junior or enterprise-minded implementation.

That means the code should communicate:

structure
consistency
separation of concerns
traceability
maintainability

The project should not look like:

one large controller
EF Core directly in controllers
random helper classes everywhere
business rules split across multiple layers with no discipline
insecure auth
no error handling
no logging
no pagination strategy
no clear API conventions
Git and Workflow Expectations

This project should be developed in an enterprise-like style.

Branching

Use a simple structured branching model:

main for stable/demo-ready state
develop for integration
feature branches for isolated work

Example feature branches:

feature/setup-solution
feature/device-crud-api
feature/auth-jwt
feature/device-assignment
feature/ai-description-generator
feature/search-filter-sort-pagination
feature/logging-error-handling
Commits

Use clear commit messages.

Good examples:

chore: create initial solution structure
feat: add generic repository and unit of work
feat: implement device CRUD endpoints
feat: add JWT authentication
feat: implement device assignment rules
feat: add global exception handling middleware
feat: implement correlation id middleware
test: add unit tests for device service

Avoid vague commit messages like:

update
fix
work
final
Pull Request Mindset

Even if working alone, code should be organized as if it could be reviewed by another developer.

What Copilot Must Avoid

When generating code for this project, do not:

violate the project reference rules
put business logic in controllers
access DbContext directly from controllers
use insecure password hashing
hardcode secrets
expose AI provider keys to the frontend
create unnecessarily complex abstractions
introduce architecture that does not match the existing layered approach
add random packages without a clear reason
generate giant service classes with unrelated responsibilities
generate giant DTOs for unrelated scenarios
use dynamic or reflection-heavy patterns without a strong need
ignore SQL script requirements
ignore idempotency requirements
ignore correlation id requirements
ignore structured logging and global exception handling requirements
What Copilot Should Optimize For

When generating code, optimize for:

maintainability
clarity
consistency
enterprise readability
testability
good separation of concerns
clean dependency direction
secure defaults
straightforward explanation in an interview

The final project should be easy to explain in a technical interview.

Preferred Delivery Style for Generated Changes

When asked to implement something, prefer this behavior:

identify the files that need to be created or changed
keep changes aligned with the current architecture
avoid introducing unnecessary files
implement the feature end-to-end in a clean way
preserve naming consistency
preserve dependency direction
keep controllers thin
keep services cohesive
make DTOs explicit
keep code interview-friendly

If a feature touches multiple layers, generate the required changes across all necessary layers in a coherent way.

Backend-First Delivery Priority

Until Angular is implemented, prioritize backend delivery in this order:

solution structure and project references
domain entities and enums
DbContext and configurations
repositories and unit of work
service layer and DTOs
controllers
validation
exception handling middleware
logging and correlation id
JWT auth
assignment/unassignment
search/filter/sort/pagination pipeline
SQL scripts
tests
AI integration
frontend integration later

If asked to choose between architectural polish and completing required assignment functionality, prefer completing required functionality cleanly.

Final Standard

Every generated change should support this goal:

Build a realistic, clean, layered, enterprise-style Device Management System that satisfies the assignment requirements and presents the developer as organized, security-aware, architecture-aware, and capable of working in a professional codebase.