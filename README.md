**Live Website:** https://acxiomcrm.runasp.net
# AcxiomCRM - implementation project

This is a deadline-oriented ASP.NET Core MVC implementation pack based on the supplied 19-page AcxiomCRM specification.

## Technology
- ASP.NET Core MVC / C#
- .NET 8
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Bootstrap 5
- Chart.js

## Included features
- Identity registration/login/logout infrastructure and role seeding (Admin, Manager, SalesExecutive)
- Customer create/list/edit/deactivate-delete flow and duplicate email/phone checks
- Lead create/list
- Opportunity create/list with amount, probability and close-date validation
- Follow-up create/list
- Activity create/list/edit/delete
- Dashboard KPI cards and chart
- Reports summary page
- Admin-only audit log page
- Authenticated REST API for customer GET/GET-by-id/POST/PUT/DELETE and lead/opportunity GET/POST
- Data annotations and antiforgery validation on MVC POST forms

## Setup
1. Install the .NET 8 SDK (not just the runtime).
2. Install SQL Server Express or LocalDB. If using another SQL Server instance, edit `appsettings.json`.
3. Open this folder in VS Code or Visual Studio.
4. In the terminal, run:
   `dotnet restore`
   `dotnet run`
5. Open the localhost URL printed in the terminal.
6. Register a user through the Identity Register page.

## Database
The project currently uses `EnsureCreated()` for fast local database creation. For a formal migration-based workflow, use:
- `dotnet tool install --global dotnet-ef`
- `dotnet ef migrations add InitialCreate`
- `dotnet ef database update`

Do not run `EnsureCreated()` and migrations together on the same database. If switching to migrations, remove the `EnsureCreated()` startup call first.

## Important before submission
This package is an implementation baseline, not a verified production-complete delivery. The SDK/runtime and SQL Server were not available in the creation environment, so compilation and end-to-end testing must be performed on the target computer.

The specification's remaining items to verify/finish include:
- Admin workflow for assigning roles to users and a safe first-admin bootstrap process
- Strict owner/team-level filtering for every MVC page, report and API
- Lead conversion workflow and status-transition rules
- Full edit/deactivate workflows for leads, opportunities and follow-ups
- All audit events (login success/failure, role changes, security events, record before/after values)
- Export, pagination and complete report filtering
- Automated tests and final security review
- Confirm database unique constraints and friendly handling of uniqueness exceptions

Never claim a requirement is complete unless you have run and demonstrated its acceptance test.


## Local demo login
- Email: `admin@acxiomcrm.local`
- Password: `Admin@12345`

This demo account is created on first launch and assigned the Admin role. Change/remove it before any deployment outside a local evaluation environment.

## Troubleshooting
- `dotnet is not recognized`: install the .NET 8 SDK (x64), then close and reopen VS Code/PowerShell.
- SQL Server connection error: install SQL Server Express LocalDB, or change `DefaultConnection` in `appsettings.json` to your installed SQL Server instance.
- If NuGet restore fails, check internet connectivity and run `dotnet restore` again.
