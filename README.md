# PolicyGate API

In-memory ASP.NET Core POC for AI identity security compliance. It evaluates prompts, applies identity/role, prompt-injection, and sensitive-data rules, stores audit events in memory, and exposes dashboard metrics.

## Prerequisites

- .NET 10 SDK

## Run

```bash
dotnet restore PolicyGate.Api.slnx
dotnet run --project src/PolicyGate.Api
```

Swagger/OpenAPI is available from the URL printed by ASP.NET Core. The API permits the Angular development origin `http://localhost:4200`.

## Demo identity headers

This POC intentionally uses header-based demo identity rather than a real identity provider:

- `X-User-Id`: user identifier
- `X-User-Roles`: comma-separated roles, such as `AIReader,AIAdmin`

Replace `DemoHeaderAuthenticationHandler` with OIDC/JWT before production use.

## Test and coverage

```bash
dotnet test PolicyGate.Api.slnx --collect:"XPlat Code Coverage" --results-directory TestResults

dotnet tool install --global dotnet-reportgenerator-globaltool
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:"Html;Cobertura;TextSummary"
```

Coverage focuses on the rule engine, repositories, service behavior, HTTP endpoints, validation, and authentication paths.

## Main endpoints

- `POST /api/security/evaluate`
- `GET /api/audit`
- `GET /api/dashboard`
- `GET /api/rules`
- `PUT /api/rules/{ruleId}` (requires `AIAdmin`)
- `DELETE /api/audit` (requires `AIAdmin`)

## Important limitation

All state is in memory and is lost when the API restarts. This is intentional for the POC.
