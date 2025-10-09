# GeologicalProperties.Service

## Overview
- ASP.NET Core 8 microservice that exposes CRUD-style REST endpoints for geological property datasets and interpolation cases.
- Hosts Swagger UI under `/GeologicalProperties/api/swagger/` and serves the bundled OpenAPI schema generated from the domain model.
- Persists data in an on-disk SQLite database stored in `../home/GeologicalProperties.db`, including light/heavy projections of the model objects.

## Runtime Architecture
- `Program.cs` wires up MVC controllers, Json serialization conventions, path base (`/GeologicalProperties/api`), CORS, and the custom Swagger document pipeline.
- `SqlConnectionManager` (singleton) provisions the SQLite database, enforces the expected table schema, and hands out short-lived connections per request.
- `GeologicalPropertiesManager` and `GeologicalPropertiesInterpolationCaseManager` encapsulate SQL access, JSON (de)serialization to the shared model types, and business rules such as interpolation workflows.
- REST controllers (`Controllers/GeologicalPropertiesController.cs`, `Controllers/GeologicalPropertiesInterpolationCaseController.cs`) forward HTTP requests to their respective managers, returning `GeologicalProperties`, `GeologicalPropertiesLight`, and interpolation case DTOs from the `Model` project.
- `DatabaseCleanerService` runs as a hosted background service, purging records whose `LastModificationDate` is older than 90 days so the SQLite file stays manageable.
- `SwaggerMiddlewareExtensions` reloads the merged OpenAPI document on each request and rewrites the server URL to reflect reverse-proxy headers before serving it to clients.

## Dependencies
- References `Model/Model.csproj` for the core domain classes that the API serializes.
- Depends on Swashbuckle packages for OpenAPI generation and on `Microsoft.Data.Sqlite` for persistence (`Service/Service.csproj`).
- Consumes the merged OpenAPI bundle placed in `wwwroot/json-schema/GeologicalPropertiesMergedModel.json`; the document is regenerated during `dotnet build` of the Service project and later reused by downstream tooling.

## Endpoints (Highlights)
- `/GeologicalProperties` → list and retrieve heavy data sets, plus light-weight metadata projections (`.../LightData`, `.../HeavyData`, `.../{id}`).
- `/GeologicalPropertiesInterpolationCase` → manage interpolation case metadata and heavy definitions, calculate interpolated/extrapolated property tables.
- Swagger UI available at `/GeologicalProperties/api/swagger/index.html` with the merged schema served from `/GeologicalProperties/api/swagger/merged/swagger.json`.

## Build & Run
```bash
dotnet build Service/Service.csproj
dotnet run --project Service/Service.csproj
```
- Database files are created automatically under `home/` relative to the solution root. Remove the folder to reset local data.
- Tests in `ServiceTest` exercise the public API using the shared DTOs.

## How Other Projects Consume This Service
- `ModelSharedOut` relies on the service emitting `wwwroot/json-schema/GeologicalPropertiesMergedModel.json`; its tooling merges this document to generate the distributed shared model consumed by clients.
- `ServiceTest` references the shared model output to validate API behaviour without re-declaring DTOs.
- `WebApp` uses the same generated shared types to call the service endpoints and display results in the UI.
- External consumers integrate through the published Swagger endpoint, reusing the same schema that backs `ModelSharedOut`.

## Contributors
- **Eric Cayeux**, *NORCE Energy Modelling and Automation*
- **Lucas Volpi**, *NORCE Energy Modelling and Automation*
