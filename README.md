# PCPartsStore

## Overview
PCPartsStore is a straightforward e-commerce app for computer components. It lets users easily browse and buy varios products. For administrators, there's a basic tool to manage inventory.

## Features
- User-friendly e-commerce platform for computer components.
- Admin panel for easy product management.

## Prerequisites
- [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Entity Framework](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [PostgreSQL](https://www.postgresql.org/download/)
- C# IDE: Visual Studio or Jetbrains Rider

## Installation
1. Clone the repository: `git clone https://github.com/Exonault/PCPartsStore`.
2. Open the project inside your IDE.
3. In the `appsettings.json` in the field `ApplicationDb` add your database connection string.
4. In the terminal execute `dotnet restore` to install any missing dependencies.
5. Apply migrations: In the terminal execute `dotnet ef update database`. 
6. Press run.
