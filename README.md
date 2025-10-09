# GeologicalProperties

## Overview
- Full-stack solution for storing, interpolating, and visualising geological rock properties along wellbores.
- Centred around a .NET 8 microservice (`Service`) backed by a shared domain model (`Model`) and exposed through a Blazor Server client (`WebApp`).
- Shared OpenAPI tooling (`ModelSharedOut`) keeps all consumers synchronised with the service contract; automated tests (`ModelTest`, `ServiceTest`) cover the core logic and API paths.

## Project Map

| Project / Folder | Type | Purpose | Depends On | Consumed By |
|------------------|------|---------|------------|-------------|
| `Model/` | Class library | Core domain entities, interpolation logic, helper types for geological property tables. | OSDC.DotnetLibraries packages | `Service`, `ModelTest`, shared-model generation pipeline |
| `Service/` | ASP.NET Core microservice | REST API for CRUD operations on geological properties and interpolation cases, SQLite persistence, Swagger hosting. | `Model` | External clients, `ModelSharedOut` (Swagger generation), `ServiceTest` |
| `ModelSharedOut/` | Console tooling | Bundles OpenAPI documents and generates C# shared DTOs used by client applications (distributed shared model). | `Service` swagger output, external schemas | `WebApp`, `ServiceTest`, external consumers |
| `ModelSharedIn/` | Console tooling (placeholder) | Holds generated client models for dependencies that the service may consume (e.g., external Digiwells microservices). | External OpenAPI schemas | `Service` (when remote dependencies are pulled in) |
| `WebApp/` | Blazor Server app | UI for browsing, editing, and plotting geological property datasets via the service API; includes Helm chart and Dockerfile. | `ModelSharedOut` DTOs/clients, MudBlazor, Plotly | End users |
| `ModelTest/` | NUnit test project | Verifies numerical interpolation/extrapolation behaviour and domain model invariants. | `Model` | CI quality gates |
| `ServiceTest/` | NUnit test project | Exercices the service API using the generated shared client to validate endpoints and data contracts. | `ModelSharedOut` | CI quality gates |
| `home/` | Runtime data | Holds the SQLite database `GeologicalProperties.db` used by the service (auto-created). | n/a | `Service` |

Every project carries an in-depth README describing its responsibilities and usage.

## How the Pieces Fit Together
- **Domain first:** `Model` defines the canonical geological property structures (light/heavy variants, interpolation cases, and helper types).
- **Service layer:** `Service` exposes these types over HTTP, persisting payloads to SQLite via manager classes. During `dotnet build`, it emits the merged Swagger document (`CreateSwaggerJson` MSBuild target) into `ModelSharedOut/json-schemas/`.
- **Shared model generation:** Running `ModelSharedOut` merges the service’s Swagger bundle with optional dependency schemas, producing C# DTOs (`GeologicalPropertiesMergedModel.cs`) and republishing the bundled OpenAPI document into `Service/wwwroot/json-schema/` for Swagger UI.
- **Clients:** `WebApp`, `ServiceTest`, and any third-party integrations reference the generated shared model to stay aligned with the API contract without duplicating types.
- **Testing:** `ModelTest` focuses on numerical accuracy, while `ServiceTest` checks the end-to-end REST surface using the same generated classes the UI relies on.

```
Model  ─▶  Service  ─┬─▶ Swagger bundle ─▶ ModelSharedOut ─▶ Shared DTOs ─┬─▶ WebApp
                     │                                                     └─▶ ServiceTest / External clients
                     └─▶ SQLite (home/GeologicalProperties.db)
```

## Getting Started
```bash
dotnet --version    # ensure .NET 8 SDK
dotnet restore      # restore the full solution
dotnet build GeologicalProperties.sln
```

Run the microservice locally:
```bash
dotnet run --project Service/Service.csproj
```

Regenerate the shared model after API changes:
```bash
dotnet run --project ModelSharedOut/ModelSharedOut.csproj
```

Launch the Blazor web app (assumes the service is running and configuration endpoints are set):
```bash
dotnet run --project WebApp/WebApp.csproj
```

Execute tests:
```bash
dotnet test ModelTest/ModelTest.csproj
dotnet test ServiceTest/ServiceTest.csproj
```

## Deployment
- Docker images for the microservice and web app are published under the Digiwells organisation: https://hub.docker.com/?namespace=digiwells
- Kubernetes/Helm manifests for the client live under `WebApp/charts/`; ingress paths align with the runtime base paths configured in `Program.cs`.
- Hosted environments:
  - Service (Swagger UI)  
    - https://dev.digiwells.no/GeologicalProperties/api/swagger  
    - https://app.digiwells.no/GeologicalProperties/api/swagger
  - Web application  
    - https://dev.digiwells.no/GeologicalProperties/webapp/GeologicalProperties  
    - https://app.digiwells.no/GeologicalProperties/webapp/GeologicalProperties

## Data & Security Notes
- Persistence uses a single SQLite database stored as clear text under `home/GeologicalProperties.db`.
- Authentication/authorization are not enabled by default; protect deployments via network policies, ingress authentication, or reverse proxies as needed.
- For guidance on securing and operating the containers, see the DrillingAndWells wiki: https://github.com/NORCE-DrillingAndWells/DrillingAndWells/wiki

## Documentation Cross-Links
- `Model/README.md` – domain model details and interpolation logic.
- `Service/README.md` – microservice architecture, persistence, and Swagger middleware.
- `WebApp/README.md` – UI architecture, configuration, and deployment notes.

## Funding
- Funded by the [Research Council of Norway](https://www.forskningsradet.no/) and [Industry partners](https://www.digiwells.no/about/board/) through the [SFI Digiwells (2020-2028)](https://www.digiwells.no/) initiative on Digitalization, Drilling Engineering, and GeoSteering.

## Contributors
- **Eric Cayeux**, *NORCE Energy Modelling and Automation*
- **Lucas Volpi**, *NORCE Energy Modelling and Automation*
