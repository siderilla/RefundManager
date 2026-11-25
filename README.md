# REFUNDMANAGER (backend)

The ASP.NET Core API behind [RefundManager](https://github.com/siderilla/refund-manager), the Angular app it feeds (˶ᵔ ᵕ ᵔ˶)

Handles employees, categories, outlays and refund requests, with endpoints for budget, calendar and day summaries used by the dashboard's heatmap and charts.

# Made with

- ASP.NET Core 10 (Web API) + Entity Framework Core
- PostgreSQL via Npgsql
- Swagger / OpenAPI for exploring the endpoints

# Run it

Set your own local connection string with User Secrets instead of hardcoding it:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=refundmanager;Username=...;Password=..."
```

Then apply the migrations and start the API:

```bash
dotnet ef database update
dotnet run --launch-profile https
```

Swagger UI at `/swagger`. CORS defaults to `http://localhost:4200` (see `Cors:AllowedOrigins` in `appsettings.json`), so it's meant to run alongside the [Angular frontend](https://github.com/siderilla/refund-manager) locally. When deploying, override it with an environment variable instead of editing the code, e.g. `Cors__AllowedOrigins__0=https://your-deployed-frontend.example.com`.

You'll also need a JWT signing key, set the same way (never committed to `appsettings.json`):

```bash
dotnet user-secrets set "Jwt:Key" "$(openssl rand -base64 48)"
dotnet user-secrets set "Jwt:Issuer" "RefundManager"
dotnet user-secrets set "Jwt:Audience" "RefundManagerClient"
```

# Deploy

Ships with a `Dockerfile` (multi-stage: SDK to publish, ASP.NET runtime to run), so any Docker-based host works — the public demo runs on [Render](https://render.com/). The app applies pending EF Core migrations itself on startup, so there's no manual `dotnet ef database update` step on the deploy target. Configure it with environment variables (no `appsettings.json` edits needed):

```bash
ConnectionStrings__DefaultConnection=Host=...;Port=5432;Database=...;Username=...;Password=...
Jwt__Key=...
Jwt__Issuer=RefundManager
Jwt__Audience=RefundManagerClient
Cors__AllowedOrigins__0=https://your-deployed-frontend.example.com
SeedDemoData=true
```

# Try the demo

The 4 seed employees all share the same password: **`Demo2026!`**. Log in from the [frontend](https://github.com/siderilla/refund-manager) with any of their name/surname pairs (see `employee` table) + that password. The one with the `HR Manager` role can approve/reject refund requests; the others can't (`403` if they try).

On the public deployment, the `SeedDemoData` setting is turned on (`SeedDemoData=true` as an env var). When it's on, the API makes sure the 4 demo employees/categories exist and keep a usable `Demo2026!` password, and **regenerates the demo expenses/refunds on every startup** with dates relative to the current day — so the dashboard's calendar and charts always have something to show in the current month, however long it's been since the last restart. It's off by default (including in `Development`), so a local `dotnet run` never touches your own test data.

# Security scope

Real password hashing (`PasswordHasher<Employee>`, PBKDF2 under the hood) + JWT bearer auth + server-side role checks on the two sensitive actions (refund approval, creating employees). Deliberately **not** implemented, as a scoping choice for a portfolio project rather than an oversight: login rate-limiting/lockout, 2FA, refresh-token rotation, password complexity rules, audit logging. A real production deployment handling real financial data would need those.

Enjoy ⋆｡°✩
