# GeologicalProperties.WebApp

## Overview
- Blazor Server UI that visualises and manages geological property datasets served by the `Service` microservice.
- Provides CRUD workflows for both raw geological properties and interpolation cases, including plotting and unit conversion helpers.
- Ships as a Docker image (`geologicalpropertiesgeologicalpropertieswebappclient`) and a Helm chart for Kubernetes deployment (`charts/norcedrillinggeologicalpropertieswebappclient`).

## Architecture
- `Program.cs` configures Razor pages, Blazor Server, MudBlazor components, Plotly plotting support, forwarded headers, and applies the `/GeologicalProperties/webapp` path base.
- `Configuration.cs` stores service endpoint overrides that are populated from `appsettings.*.json` at startup.
- `Shared/APIUtils.cs` centralises HTTP clients for the GeologicalProperties, Field, Cluster, Well, WellBore, and UnitConversion microservices; it instantiates the generated OpenAPI client from `ModelSharedOut`.
- `Shared/DataUtils.cs` and `Shared/PseudoConstructors.cs` prepare DTO instances and mappings between service models and UI widgets.
- Razor pages under `Pages/` (`GeologicalPropertiesMain`, `GeologicalPropertiesView`, `GeologicalPropertiesAdd`, plus interpolation case counterparts) implement the main screens, invoking MudBlazor dialogs (e.g., `Components/DialogDeleteTemplate.razor`) and Plotly charts (`Components/ScatterPlot.razor`) to render results.

## External Dependencies
- References `ModelSharedOut/ModelSharedOut.csproj` for the distributed shared model and OpenAPI-generated client classes, ensuring DTOs stay in sync with the backend schema.
- Uses MudBlazor for theming and components, Plotly.Blazor for charting, and OSDC.UnitConversion packages for drilling unit conversions (`WebApp/WebApp.csproj`).
- Consumes the REST API hosted by the `Service` project; the default URLs point to the Digiwells environments but can be overridden per deployment.

## Configuration
- Runtime URLs for partner microservices are set in `appsettings.{Environment}.json` and forwarded into the static `Configuration` class at startup.
- `APIUtils` disables certificate validation for test environments; adjust or remove the handler when promoting to production.
- The Helm chart templates mirror the ASP.NET `UsePathBase` configuration—ensure ingress paths match `/GeologicalProperties/webapp`.

## Build & Run
```bash
dotnet build WebApp/WebApp.csproj
dotnet run --project WebApp/WebApp.csproj
```
- Browse to `https://localhost:5001/GeologicalProperties/webapp` (path base required) after applying matching configuration values.
- Docker image builds via the provided `WebApp/Dockerfile`; Helm packaging lives under `WebApp/charts/`.

## Relationship to Other Projects
- Depends on `ModelSharedOut` for generated DTOs/clients and on the `Service` microservice to fetch and mutate geological property data.
- `ServiceTest` reuses the same shared model ensuring parity with the UI; changes to the model propagate here automatically through regenerated shared types.
- The `Service` project publishes the OpenAPI document consumed by `ModelSharedOut`, which this web app uses indirectly through the generated client.

## Deployment Endpoints
- Swagger UI for the backing API: `https://dev.digiwells.no/GeologicalProperties/api/swagger` (development), `https://app.digiwells.no/GeologicalProperties/api/swagger` (production).
- Web application: `https://dev.digiwells.no/GeologicalProperties/webapp/GeologicalProperties`, `https://app.digiwells.no/GeologicalProperties/webapp/GeologicalProperties`.
- Docker Hub organisation: `https://hub.docker.com/?namespace=digiwells`.

## Funding
- Funded by the [Research Council of Norway](https://www.forskningsradet.no/) and [Industry partners](https://www.digiwells.no/about/board/) within the [SFI Digiwells (2020-2028)](https://www.digiwells.no/) programme on Digitalization, Drilling Engineering, and GeoSteering.

## Contributors
- **Eric Cayeux**, *NORCE Energy Modelling and Automation*
- **Lucas Volpi**, *NORCE Energy Modelling and Automation*
