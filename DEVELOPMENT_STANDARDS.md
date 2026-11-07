# Nanma ASP.NET Core Development Standards

Applies to the Nanma ASP.NET Core Web API, a rewrite of the Nanma Ruby on Rails API (.NET 10).

## Architecture
- Use Modular Monolith architecture with Clean Architecture principles.
- Organize code by business modules (for example Auth, Users, Complaints, Payments, Vendors, Watchman, Admin, Reports).
- Keep Domain independent of infrastructure.
- Application contains use cases and business workflows.
- Infrastructure contains database and external service implementations.
- API handles HTTP requests and responses.
- Enforce module boundaries. Modules communicate through interfaces, never by reaching into another module's internals.

## C# Coding Standards
- Use modern C# features appropriately.
- Use async/await for I/O operations.
- Use CancellationToken in asynchronous application workflows.
- Use constructor injection.
- Prefer interfaces for external dependencies and persistence abstractions.
- Use meaningful class, method and variable names.
- Avoid unnecessary static classes.
- Avoid giant service classes.
- Follow SOLID principles.

## Application Development
- Use request and response DTOs.
- Separate commands from queries where appropriate.
- Use FluentValidation for complex input validation.
- Keep controllers thin.
- Use centralized exception handling.
- Implement structured logging.
- Handle null values and edge cases.
- Use appropriate HTTP status codes.
- Avoid exposing database entities directly through APIs.

## Database
- Preserve existing database structure during initial migration.
- Use explicit transactions when multiple operations must succeed or fail together.
- Avoid N+1 queries.
- Avoid unnecessary database calls.
- Use parameterized queries.
- Implement pagination for large datasets.
- Analyze indexes and query performance.
- Respect existing foreign keys and constraints.

## Code Generation Rules

### Before generating code
1. Inspect existing architecture and relevant files.
2. Understand the business requirement.
3. Identify existing reusable implementations.
4. Identify dependencies and required changes.
5. Explain the implementation approach.

### During generation
1. Generate only the necessary files.
2. Follow existing naming conventions.
3. Avoid duplicated logic.
4. Avoid unnecessary NuGet packages.
5. Avoid speculative abstractions.
6. Ensure dependency injection registrations are included.
7. Include error handling and validation.
8. Include appropriate tests.

### After generation
1. Build the solution.
2. Run relevant tests.
3. Fix compilation errors.
4. Report changed files.
5. Explain important implementation decisions.

Never claim that code is production-ready merely because it compiles.
