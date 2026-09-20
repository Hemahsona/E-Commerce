[README.md](https://github.com/user-attachments/files/32432218/README.md)
# E-Commerce

A backend API for an e-commerce platform, built with ASP.NET Core on .NET 10 and organised around Clean Architecture. It covers the product catalog, authentication, Redis-backed baskets, order placement, and Stripe payments.

## Tech stack

| Concern | Choice |
| --- | --- |
| Framework | .NET 10 / ASP.NET Core |
| Architecture | Clean Architecture (API, Application, Domain, Infrastructure) |
| Database | SQL Server |
| ORM | Entity Framework Core — migrations live in `E-Commerce.Infrastructure` |
| Authentication | ASP.NET Core Identity with JWT bearer tokens |
| Basket / caching | Redis via StackExchange.Redis |
| Payments | Stripe Payment Intents |
| API documentation | Built-in ASP.NET Core OpenAPI (`AddOpenApi()` / `MapOpenApi()`) |
| Load testing | k6 |
| Local infrastructure | Docker (SQL Server, Redis) |

## Architecture

Dependencies point inward. The Domain layer knows nothing about the outside world; Infrastructure and API depend on abstractions defined further in.

```
┌─────────────────────────────────────────┐
│           E-Commerce.API                │  Endpoints, middleware, DI wiring
│                                         │
│  ┌───────────────────────────────────┐  │
│  │     E-Commerce.Application        │  │  Use cases, DTOs, validation,
│  │                                   │  │  service interfaces
│  │      ┌─────────────────────┐      │  │
│  │      │  E-Commerce.Domain  │      │  │  Entities, value objects,
│  │      │                     │      │  │  business rules
│  │      └─────────────────────┘      │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
              ▲
              │ implements
┌─────────────────────────────────────────┐
│       E-Commerce.Infrastructure         │  EF Core, Identity, Redis,
│                                         │  Stripe, repositories
└─────────────────────────────────────────┘
```

| Project | Responsibility |
| --- | --- |
| `E-Commerce.Domain` | Entities, value objects, and business invariants. No external dependencies. |
| `E-Commerce.Application` | Use cases and orchestration. Defines the interfaces that Infrastructure implements. |
| `E-Commerce.Infrastructure` | EF Core `DbContext` and migrations, Identity stores, Redis basket repository, Stripe integration. |
| `E-Commerce.API` | ASP.NET Core entry point: endpoints, request pipeline, authentication, OpenAPI, dependency registration. |

## Features

- **Catalog** — products with filtering, sorting, and pagination
- **Authentication** — registration and login through ASP.NET Core Identity, issuing JWT bearer tokens
- **Basket** — stored in Redis rather than SQL Server, keyed per user or session
- **Orders** — order placement from a basket, with addresses and delivery methods
- **Payments** — Stripe Payment Intents created and updated alongside the basket

## Repository layout

```
E-Commerce/
├── E-Commerce.API/              # ASP.NET Core Web API
├── E-Commerce.Application/      # Use cases and application services
├── E-Commerce.Domain/           # Core domain model
├── E-Commerce.Infrastructure/   # EF Core, Identity, Redis, Stripe
├── LoadTests/                   # k6 load testing scripts
├── postman/                     # Postman collection and environments
├── .postman/                    # Postman workspace configuration
└── E-Commerce.slnx              # Solution file (XML format)
```

## Getting started

### Prerequisites

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) — the `.slnx` solution format requires a recent SDK
- [Docker](https://www.docker.com/) for SQL Server and Redis
- A [Stripe](https://stripe.com/) account in test mode, for payment endpoints
- Optional: [Postman](https://www.postman.com/) and [k6](https://k6.io/)

### 1. Start the infrastructure

```bash
docker run -d --name ecommerce-sql \
  -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Your_password123" \
  -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest

docker run -d --name ecommerce-redis \
  -p 6379:6379 redis:latest
```

### 2. Clone and restore

```bash
git clone https://github.com/Hemahsona/E-Commerce.git
cd E-Commerce
dotnet restore
```

### 3. Configure

Keep secrets out of `appsettings.json` — use user secrets in development:

```bash
cd E-Commerce.API
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=ECommerce;User Id=sa;Password=Your_password123;TrustServerCertificate=True;"
dotnet user-secrets set "ConnectionStrings:Redis" "localhost:6379"
dotnet user-secrets set "Jwt:Key" "<a long random signing key>"
dotnet user-secrets set "StripeSettings:SecretKey" "sk_test_..."
dotnet user-secrets set "StripeSettings:WebhookSecret" "whsec_..."
```

Key names should match those read in `Program.cs` and the Infrastructure service registrations — adjust if yours differ.

### 4. Apply migrations

```bash
dotnet ef database update \
  --project E-Commerce.Infrastructure \
  --startup-project E-Commerce.API
```

If the EF Core CLI is missing: `dotnet tool install --global dotnet-ef`.

To create a new migration:

```bash
dotnet ef migrations add <Name> \
  --project E-Commerce.Infrastructure \
  --startup-project E-Commerce.API
```

### 5. Run

```bash
dotnet run --project E-Commerce.API
```

The listening URLs come from `E-Commerce.API/Properties/launchSettings.json`.

## API documentation

The API uses the OpenAPI support built into ASP.NET Core rather than Swashbuckle. With the app running in Development, the generated document is served at:

```
/openapi/v1.json
```

Point any OpenAPI client at that URL, or import it into Postman or Scalar to browse the endpoints.

The `postman/` directory also contains a ready-made collection and environment files. Import both, select the environment, and the requests are set up to run against a local instance.

### Authenticating

Call the login endpoint to obtain a JWT, then send it on subsequent requests:

```
Authorization: Bearer <token>
```

## Load testing

k6 scripts live in `LoadTests/`. With k6 installed and the API running:

```bash
k6 run LoadTests/<script>.js
```

Override the target with an environment variable if the script supports one, for example `k6 run -e BASE_URL=http://localhost:5000 LoadTests/<script>.js`.

## Stripe webhooks

Payment Intents need Stripe to reach the webhook endpoint. For local development, forward events with the Stripe CLI:

```bash
stripe listen --forward-to https://localhost:5001/api/payments/webhook
```

The CLI prints a signing secret — set it as `StripeSettings:WebhookSecret`.

## Contributing

1. Fork the repository and branch off `master`.
2. Keep changes in the right layer: business rules belong in `E-Commerce.Domain`, orchestration in `E-Commerce.Application`, and anything touching SQL Server, Redis, or Stripe in `E-Commerce.Infrastructure`.
3. Open a pull request describing the change and how to verify it.

