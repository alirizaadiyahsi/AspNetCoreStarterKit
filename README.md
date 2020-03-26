# Asp Net Core Starte rKit

Modular monolithic Web API startup template.

<br/>
<br/>

## Requirements

- Visual Studio 2019 v16.3 or later **for Windows**.
- Visual Studio 2019 for Mac v8.3 or later **for macOS**.

## How to Start?

### Local Environment

- Select `AspNetCoreStarterKit.WebApi` project "**Set as Startup Project**"
- Open "**Package Manager Console**" and select default project as `src/AspNetCoreStarterKit.EntityFramework`
- Run `update-database` command to create database.
- Run(F5 or CTRL+F5) Web API project first 
- Admin user name and password : `admin/123qwe`

## Swagger UI Authorize

You can login on swagger ui by using a bearer token. So you can make requests to authorized end-points. Check the following steps.

- In swagger ui, execute `api/login` to get a bearer token.
- Copy bearer token that is in `api/login` response.
- Click `Authorize` button in swagger ui page.
- Enter the token like `Bearer <token>` and click `Authorize`.
- Now you can make requests to authorized end-points.

###

### Tags & Technologies

- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.1)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity?view=aspnetcore-2.1)
- [JWT (Bearer Token) Based Authentication](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/)
- [Automapper](https://automapper.org/)
- [Serilog](https://serilog.net/)
- [Swagger](https://swagger.io/)
- [Authorization & Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-2.1)
- [Exception Handling & Logging](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-2.1)

## Lincense

[MIT License](https://github.com/alirizaadiyahsi/AspNetCoreStarterKit/blob/master/LICENSE)
