# .NET 10.0 (Preview) Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 10.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 10.0 upgrade.

3. Upgrade `src\NBT.Domain\NBT.Domain.csproj`
4. Upgrade `src\NBT.Application\NBT.Application.csproj`
5. Upgrade `src\NBT.Infrastructure\NBT.Infrastructure.csproj`
6. Upgrade `src\NBT.WebUI\NBT.WebUI.csproj`
7. Upgrade `src\NBT.WebAPI\NBT.WebAPI.csproj`


## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name                                   | Description                 |
|:-----------------------------------------------|:---------------------------:|


### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                                                       | Current Version | New Version | Description                                   |
|:-------------------------------------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| AutoMapper.Extensions.Microsoft.DependencyInjection                |   12.0.1        |  12.0.1     | Deprecated — functionality now included in AutoMapper; remove or validate usage. |
| FluentValidation.AspNetCore                                         |   11.3.0        |  11.3.1     | Minor update recommended.                     |
| Microsoft.AspNetCore.Authentication.JwtBearer                       |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.AspNetCore.Components.WebAssembly.Server                  |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore                   |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.AspNetCore.OpenApi                                        |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.EntityFrameworkCore                                       |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.EntityFrameworkCore.Design                                |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.EntityFrameworkCore.Relational                            |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.EntityFrameworkCore.SqlServer                             |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore   |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |
| Microsoft.Extensions.Identity.Stores                                |   9.0.0         |  10.0.0     | Recommended for .NET 10.0                     |


### Project upgrade details
This section contains details about each project upgrade and modifications that need to be done in the project.

#### src\NBT.Domain\NBT.Domain.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.Extensions.Identity.Stores` should be updated from `9.0.0` to `10.0.0` (recommended for .NET 10.0).

Other changes:
  - Review any code that depends on package behavior changes after package update.


#### src\NBT.Application\NBT.Application.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.EntityFrameworkCore` should be updated from `9.0.0` to `10.0.0` (recommended for .NET 10.0).
  - `AutoMapper.Extensions.Microsoft.DependencyInjection` is listed as deprecated at `12.0.1`; verify whether it should be removed or adjusted since AutoMapper now includes the functionality.
  - `FluentValidation.AspNetCore` should be updated from `11.3.0` to `11.3.1`.

Other changes:
  - After EF Core update, check for any breaking changes in queries, model building, or migrations.


#### src\NBT.Infrastructure\NBT.Infrastructure.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.AspNetCore.Identity.EntityFrameworkCore` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.EntityFrameworkCore` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.EntityFrameworkCore.Design` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.EntityFrameworkCore.Relational` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.EntityFrameworkCore.SqlServer` should be updated from `9.0.0` to `10.0.0`.

Other changes:
  - Validate EF Core migrations and ensure design-time tools still work after package updates.


#### src\NBT.WebUI\NBT.WebUI.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.AspNetCore.Components.WebAssembly.Server` should be updated from `9.0.0` to `10.0.0`.

Other changes:
  - Verify Blazor-specific APIs and components for any .NET 10 changes.


#### src\NBT.WebAPI\NBT.WebAPI.csproj modifications

Project properties changes:
  - Target framework should be changed from `net9.0` to `net10.0`

NuGet packages changes:
  - `Microsoft.AspNetCore.Authentication.JwtBearer` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.AspNetCore.OpenApi` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.EntityFrameworkCore.Design` should be updated from `9.0.0` to `10.0.0`.
  - `Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore` should be updated from `9.0.0` to `10.0.0`.

Other changes:
  - Verify OpenAPI/Swagger and health checks behavior after updates.


