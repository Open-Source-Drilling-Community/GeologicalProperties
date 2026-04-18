# NORCE.Drilling.GeologicalProperties.WebPages

Reusable Razor class library containing the GeologicalProperties pages and their supporting components.

## Contents
- `GeologicalPropertiesMain`, `GeologicalPropertiesAdd`, `GeologicalPropertiesView`
- `GeologicalPropertiesInterpolationCaseMain`, `GeologicalPropertiesInterpolationCaseAdd`, `GeologicalPropertiesInterpolationCaseView`
- supporting MudBlazor dialog and Plotly components
- shared helpers for DTO construction, unit handling, and conversions

## Dependencies
- `ModelSharedOut` for generated DTOs and API clients
- `OSDC.DotnetLibraries.Drilling.WebAppUtils` for host URL configuration contracts and shared API utility support

## Host Integration
Register a host configuration object in the consuming web app that implements `IGeologicalPropertiesWebPagesConfiguration`, then register `IGeologicalPropertiesAPIUtils` with `GeologicalPropertiesAPIUtils`.

The host application must also include this assembly in Blazor routing:

```csharp
typeof(NORCE.Drilling.GeologicalProperties.WebPages.Pages.GeologicalPropertiesMain).Assembly
```

## Packaging
This project is intended to be packed and published as the NuGet package `NORCE.Drilling.GeologicalProperties.WebPages`.
