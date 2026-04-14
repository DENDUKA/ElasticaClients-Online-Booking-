# ElasticaClients - Online Booking System

## Overview
Fitness studio and client management system with online booking capabilities. Built on ASP.NET MVC 5 with layered architecture.

## Technology Stack
- **Framework**: .NET Framework 4.8
- **Web Framework**: ASP.NET MVC 5.2.4 with Razor views
- **Database**: SQL Server (Express: `DENDUKA-PC\SQLEXPRESS01`, database: `FlyStretch`)
- **ORM**: Entity Framework 6.4.4 (code-first with migrations)
- **DI Container**: Microsoft.Extensions.DependencyInjection 8.0.0
- **Frontend**: Bootstrap 4.6.0, jQuery 3.5.1, FullCalendar 3.9.0, Moment.js
- **API Integration**: RestSharp 108.0.3, YClients API client
- **Telegram Bot**: Telegram.Bot 18.0.0
- **Excel Processing**: ExcelDataReader 3.6.0, IronXL.Excel

## Solution Structure

```
ElasticaClients.sln
├── ElasticClients/              # Main ASP.NET MVC 5 Web Application
├── ElasticaClients.DAL/         # Data Access Layer (Class Library)
├── ConsoleApp1/                 # Utility Console Application
├── YClientsAPI/                 # YClients API Integration (Class Library)
└── TelegramBot/                 # Telegram Bot (Console App, standalone)
```

### Projects

#### 1. ElasticClients (Web Application)
Main web UI with controllers, views, and business logic.

**Controllers** (13 total):
- AccountController, AdminController, BranchController, GymController
- HomeController, IncomeController, LogController, RoleController
- SubscriptionController, SubscriptionFreezeController
- TrainingCalendarController, TrainingController, TrainingItemController

**Key Components**:
- `Logic/` - Business logic services (AccountB, BranchB, GymB, IncomeB, etc.)
- `Infrastructure/` - DI resolver (ServiceProviderDependencyResolver)
- `Models/` - View models (AccountStat, TrainingCalendarJSON)
- `Helpers/` - Utility classes (ExcelGenerator, ExcelHelper, Batches)

#### 2. ElasticaClients.DAL (Data Access Layer)
Entity Framework entities, repositories, and migrations.

**Entities** (19 files):
- Account, Branch, Gym, Income, Role
- Subscription, Training, TrainingItem
- FreezeSubscriptionItem, AppLog
- Each with corresponding DbContext

**Structure**:
- `Entities/` - EF entities and DbContext classes
- `Data/Interfaces/` - Repository interfaces (IAccountDAL, IBranchDAL, etc.)
- `Data/` - Concrete DAL implementations + Mock implementations
- `Migrations/` - 46 code-first migrations (2020-07 to 2022-02)
- `Cache/` - AccountCache
- `Accessory/` - Enums and constants (IncomeType, Salary, SubscriptionStatus, etc.)
- `ServiceConfiguration.cs` - DI registration extensions

#### 3. ConsoleApp1 (Utility Application)
Console application for data processing and maintenance tasks.

**Features**:
- Excel processing and generation
- Subscription management
- Analytics and reporting
- EngelsAccounts integration

#### 4. YClientsAPI (API Client Library)
Integration with YClients external booking system.

#### 5. TelegramBot (Standalone Console App)
Telegram bot for notifications and interaction (not included in .sln).

## Architecture Patterns

### Layered Architecture (3-tier)

```
Presentation Layer (Controllers/Views)
        ↓
Business Logic Layer (Logic/*B classes)
        ↓
Data Access Layer (DAL/Repositories)
        ↓
Database (SQL Server via Entity Framework)
```

### Dependency Injection
- Custom `ServiceProviderDependencyResolver` for ASP.NET MVC integration
- Service configuration split:
  - `AddDataAccessServices()` - Singleton lifetime for repositories
  - `AddBusinessLogicServices()` - Scoped lifetime for business services
- Mock implementations available for all DAL interfaces

### Domain Model
The system manages a fitness/studio booking domain:
- **Accounts** - Users/staff members
- **Branches** - Physical locations
- **Gyms** - Facilities within branches
- **Subscriptions** - Client membership plans (with freeze capability)
- **Trainings** - Scheduled sessions
- **TrainingItems** - Individual training components
- **Incomes** - Financial records and transactions
- **Roles** - Authorization and permissions

## Key Dependencies

### Entity Framework Extensions
- **Z.EntityFramework.Extensions 4.0.94** - Bulk operations
- **Z.EntityFramework.Plus.EF6 1.12.34** - Query enhancements

### UI Libraries
- **FullCalendar.MVC5 1.0.6** - Calendar integration
- **Bootstrap 4.6.0** - Responsive UI
- **jQuery 3.5.1** - DOM manipulation
- **Modernizr** - Feature detection
- **Moment.js** - Date/time handling

## Build & Run

### Prerequisites
- Visual Studio 2017 or later
- .NET Framework 4.8 SDK
- SQL Server Express (or full SQL Server)
- NuGet packages restored

### Configuration
- Connection string in `ConsoleApp1/App.config` points to `DENDUKA-PC\SQLEXPRESS01`
- Database name: `FlyStretch`
- Web.config contains ASP.NET application settings

### Build Commands
```bash
# Using MSBuild
msbuild ElasticaClients.sln /p:Configuration=Debug

# Or open in Visual Studio and build
```

### Database Migrations
46 migrations available (2020-07 to 2022-02):
```bash
# Update database to latest migration
Update-Database -ProjectName ElasticaClients.DAL

# Or target specific migration
Update-Database -TargetMigration <MigrationName>
```

## Code Conventions
- Russian language comments and UI text
- Entity Framework code-first approach
- Repository pattern for data access
- Business logic separated into `*B` classes (e.g., AccountB, BranchB)
- DAL interfaces prefixed with `I` (e.g., IAccountDAL)
- Mock implementations available for testing

## Important Notes
- Packages use old `packages.config` format (not PackageReference)
- Mock DAL implementations can be switched via `ServiceConfiguration`
- Project uses Z.EntityFramework.Extensions for bulk operations
- YClients integration for external booking system sync
- Telegram bot for notifications (separate project)

## File Locations
- **Solution**: `ElasticaClients.sln`
- **Web Config**: `ElasticClients/Web.config`
- **DAL Config**: `ElasticaClients.DAL/App.config`
- **Packages**: `packages/` directory (local NuGet cache)
- **Migrations**: `ElasticaClients.DAL/Migrations/`
