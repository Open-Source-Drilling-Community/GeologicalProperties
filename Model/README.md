# GeologicalProperties.Model

## Overview
- Provides the core domain model for storing, interpolating, and extrapolating geological rock properties along a wellbore’s measured depth.
- Shipping as a `net8.0` class library (`Model/Model.csproj`) that is consumed by the service and clients in this solution.
- Supplies lightweight DTOs for listing metadata as well as full data objects that keep complete property tables for downhole analytics.

## Key Concepts
- `GeologicalProperties` & `GeologicalPropertiesLight` hold measured datasets together with metadata such as creation timestamps, wellbore identifiers, and provenance flags (`Model/GeologicalProperties*.cs`).
- `GeologicalPropertyEntry` captures rock parameters (UCS, CCS, internal friction angle, porosity, permeability, pressure differential, measured depth) plus the `GeologicalPropertyTableOrigin` enum that marks values as measured, interpolated, or extrapolated (`Model/GeologicalPropertyEntry.cs`, `Model/DerivedTypes/GeologicalPropertyTableOrigin.cs`).
- `GeologicalPropertiesInterpolationCase` orchestrates the main calculations: it normalises measured inputs, computes derived CCS values, performs linear interpolation, and optionally extrapolates the dataset by calling the helper `LSQM` least-squares routine (`Model/GeologicalPropertiesInterpolationCase*.cs`, `Model/LSQM.cs`).
- `InterpolationProperties` and `ExtrapolationProperties` describe how finely new points should be created and whether extrapolation is enabled, keeping configuration separate from raw data (`Model/InterpolationProperties.cs`, `Model/ExtrapolationProperties.cs`).

## External Dependencies
- Depends on the `OSDC.DotnetLibraries` drilling, data management, and statistics packages for Gaussian property definitions and metadata handling (`Model/Model.csproj`).
- Targets .NET 8 with nullable reference types enabled.

## Build & Test
```bash
dotnet build Model/Model.csproj
dotnet test ModelTest/ModelTest.csproj
```
- Unit tests in `ModelTest` validate interpolation and extrapolation behaviour, referencing the same model types.

## How Other Projects Use This Library
- `Service/Service.csproj` references the model to expose CRUD endpoints for geological properties; controllers return `GeologicalProperties`, `GeologicalPropertiesLight`, and interpolation case payloads directly over the API.
- `ModelSharedOut` bundles the model’s OpenAPI description and generates client-friendly DTOs so external consumers stay in sync with this library’s contract.
- `ServiceTest` and `WebApp` reference the generated shared model (`ModelSharedOut`) to exercise the API and build UI workflows without duplicating class definitions.
- Downstream consumers rely on the origin flags, metadata, and Gaussian drilling property structure from this project to distinguish measured data from generated (interpolated/extrapolated) values when plotting or validating results.

## When to Use
- Include this package whenever you need strongly typed geological property tables in the solution.
- Use the light variants (`GeologicalPropertiesLight`, `GeologicalPropertiesInterpolationCaseLight`) for list views or metadata-only queries, and switch to the heavy types when full property tables and derived statistics are required.

## Contributors
- **Eric Cayeux**, *NORCE Energy Modelling and Automation*
- **Lucas Volpi**, *NORCE Energy Modelling and Automation*
