# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

CandyKingdom is an Nx monorepo containing Angular libraries for a CMS framework and .NET backend services. Angular 21 + Nx 22.

## Architecture

- **`libs/bonnie`** (`@candy-kingdom/bonnie`): Core Angular library - data models (Bone, Skeleton, Page), localization, media/image/video handling via Marcy backend
- **`libs/bonnie-cms`** (`@candy-kingdom/bonnie-cms`): CMS UI components - skeleton editor, bone editors, admin controls, file/media uploaders, translation inputs, form controls
- **`libs/marcy`**: .NET backend library - image processing, video management, page/view models
- **`libs/marcy-cms`**: .NET CMS API controllers (pages, views, settings, uploads)
- **`apps/bonnie-cms-sample`**: Angular sample app demonstrating bonnie-cms usage
- **`apps/marcy-cms-sample`**: .NET sample app demonstrating marcy-cms usage
- **`apps/marcy-gen`**: TypeGen code generator producing TypeScript interfaces from .NET models

Path aliases: `@candy-kingdom/bonnie` → `libs/bonnie/src/index.ts`, `@candy-kingdom/bonnie-cms` → `libs/bonnie-cms/src/index.ts`

## Key Patterns

- **Signals**: The codebase uses Angular signals (`input()`, `output()`, `model()`, `signal()`, `computed()`) instead of decorators. Templates must call signals with `()` to unwrap values.
- **Control flow**: Uses `@if`/`@for` syntax instead of `*ngIf`/`*ngFor`.
- **Bone editors**: Extend `BoneEditorBaseComponent<TBone>` and implement `IBoneEditor`. The `bone` property is a `WritableSignal<TBone>` - access properties via `bone().property` in templates and `this.bone().property` in TS.
- **BoneEditorMap**: Maps bone type strings to editor component types. Uses `IBoneEditor<any>` for variance compatibility.
- **Generated types**: TypeScript interfaces in `libs/bonnie/src/lib/generated/` and `libs/bonnie-cms/src/lib/generated/` are auto-generated from .NET models via marcy-gen.

## Build Commands

```bash
npx nx run-many -t build          # Build all projects
npx nx build bonnie               # Build bonnie library
npx nx build bonnie-cms           # Build bonnie-cms library
npx nx build bonnie-cms-sample    # Build sample app (depends on both libs)
npx nx serve bonnie-cms-sample    # Dev server
npx nx lint <project>             # Lint a project
```

## Versioning

```bash
npm run version-bump              # Patch version bump + push tags
```
