# CandyKingdom

An Nx monorepo for a headless CMS framework. Angular 21 + Nx 22 on the front end; .NET 10 on the back end.

## Packages

### Angular libraries

| Package | Path | Description |
| --- | --- | --- |
| `@candy-kingdom/bonnie` | [`libs/bonnie`](libs/bonnie) | Core rendering library — components, directives, and interfaces for displaying CMS content |
| `@candy-kingdom/bonnie-cms` | [`libs/bonnie-cms`](libs/bonnie-cms) | CMS admin UI library — skeleton editor, bone editors, forms, uploaders |

### .NET libraries

| Package | Path | Description |
| --- | --- | --- |
| `CandyKingdom.Marcy` | [`libs/marcy`](libs/marcy) | Core domain models, media processing, file storage (source of truth for data contracts) |
| `CandyKingdom.MarcyCms` | [`libs/marcy-cms`](libs/marcy-cms) | ASP.NET Core CMS layer — REST API controllers, EF Core data access, settings |

### Apps

| App | Path | Description |
| --- | --- | --- |
| `bonnie-cms-sample` | [`apps/bonnie-cms-sample`](apps/bonnie-cms-sample) | Angular SSR sample app demonstrating both public rendering and admin UI |
| `marcy-cms-sample` | [`apps/marcy-cms-sample`](apps/marcy-cms-sample) | .NET sample server wiring up marcy-cms with SQLite + Identity |
| `marcy-gen` | [`apps/marcy-gen`](apps/marcy-gen) | TypeGen code-generation tool — produces TypeScript interfaces from C# models |

## How it fits together

```text
C# models (libs/marcy, libs/marcy-cms)
    │
    └─► apps/marcy-gen ──────────────────► libs/bonnie/src/lib/generated/
                                           libs/bonnie-cms/src/lib/generated/

apps/marcy-cms-sample (startup)  ────────► apps/bonnie-cms-sample/src/app/generated/

apps/marcy-cms-sample  ←──── HTTP ────────  apps/bonnie-cms-sample
```

TypeScript interfaces in any `generated/` folder are auto-generated from C# source. **Never edit them by hand.**

## Getting started

### Prerequisites

- Node.js 20+
- .NET 10 SDK

### Run the sample app

```bash
# 1. Install JS dependencies
npm install

# 2. Start the .NET backend (also regenerates TypeScript contracts on startup)
dotnet run --project apps/marcy-cms-sample

# 3. In another terminal, start the Angular dev server
npx nx serve bonnie-cms-sample
```

The Angular app proxies API calls to the .NET backend via `proxy.conf.json`.

## Build

```bash
npx nx run-many -t build          # Build all JS projects
npx nx build bonnie               # Build bonnie library only
npx nx build bonnie-cms           # Build bonnie-cms library only
npx nx build bonnie-cms-sample    # Build Angular sample app
npx nx serve bonnie-cms-sample    # Dev server with live reload
npx nx lint <project>             # Lint a project
dotnet run --project apps/marcy-gen  # Regenerate TS interfaces from C# models
```

## Versioning

```bash
npm run version-bump              # Patch bump + push tags
```
