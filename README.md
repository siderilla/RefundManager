# REFUNDMANAGER (backend)

The ASP.NET Core API behind [RefundManager](https://github.com/siderilla/refund-manager), the Angular app it feeds (˶ᵔ ᵕ ᵔ˶)

Handles employees, categories, outlays and refund requests, with endpoints for budget, calendar and day summaries used by the dashboard's heatmap and charts.

# Made with

- ASP.NET Core 8 (Web API) + Entity Framework Core
- PostgreSQL via Npgsql
- Swagger / OpenAPI for exploring the endpoints

# Run it

Set your own local connection string with User Secrets instead of hardcoding it:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Postgres" "Host=localhost;Port=5432;Database=refundmanager;Username=...;Password=..."
```

Then apply the migrations and start the API:

```bash
dotnet ef database update
dotnet run
```

Swagger UI at `/swagger`. CORS is scoped to `http://localhost:4200`, so it's meant to run alongside the [Angular frontend](https://github.com/siderilla/refund-manager).

Enjoy ⋆｡°✩
