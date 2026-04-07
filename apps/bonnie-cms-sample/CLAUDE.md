# apps/bonnie-cms-sample — CLAUDE.md

Angular 21 SSR reference app. Demonstrates public page rendering and CMS admin UI backed by `apps/marcy-cms-sample`. See the root `CLAUDE.md` for full context.

## Running

```bash
# Start the .NET backend first (also regenerates src/app/generated/)
dotnet run --project apps/marcy-cms-sample

# Then start the Angular dev server
npx nx serve bonnie-cms-sample
```

API calls are proxied to the .NET backend via `proxy.conf.json`.

## Critical rule

**Never edit `src/app/generated/`** — those files are written by `marcy-cms-sample` on every server startup from `MarcyCmsSampleGenerationSpec`. Changes will be lost.

## Adding a new bone type

This requires changes in **four places**:

**1 — Public bone component** in `src/app/bone-components/`:

```typescript
@Component({ selector: 'app-my-bone', /* ... */ })
export class MyBoneComponent implements IBoneComponent {
  readonly bd = inject(BoneDirective<MyBone>);
}
```

**2 — Bone editor component** in `src/app/bone-editors/`:

```typescript
@Component({ selector: 'app-my-bone-editor', /* ... */ })
export class MyBoneEditorComponent extends BoneEditorBaseComponent<MyBone> {
  getPresets(): ContentPreset<MyBone>[] {
    return [createPreset({ title: 'Default', style: MyBoneStyle.default })];
  }
}
```

**3 — Register in `FaceBoneMap.ts`** (public rendering):

```typescript
export const FaceBoneMap = new BoneMap([
  // ...existing
  ['my-bone', MyBoneComponent],
]);
```

**4 — Register in `AdminBoneMap.ts`** (CMS editing):

```typescript
export const AdminBoneMap: BoneEditorMap = new Map([
  // ...existing
  ['my-bone', MyBoneEditorComponent],
]);
```

Also add a factory function in `src/app/core/` (e.g. `emptyMyBone()`) and register it as a template in the admin page editor.

## Adding a bone template (for the "add bone" menu in admin)

In `src/app/admin-pages/admin-pages.component.ts`, add to the `templates` array:

```typescript
readonly templates: IBoneTemplate[] = [
  // ...existing
  { title: 'My Bone', boneFactory: () => emptyMyBone() },
];
```

## Route structure

| Route | Component | Notes |
| --- | --- | --- |
| `/signin` | Sign-in form | Public |
| `/register` | Register form | Public |
| `/admin/**` | Admin shell | Lazy, guarded by `AuthGuard` |
| `/**` | `FaceComponent` | SSR catch-all; resolves page + view + settings |

## Localization

`RouterLocalizeService` derives the active locale from the URL path prefix (e.g. `/en/...`, `/ru/...`). It extends `LocalizeServiceBase` and is provided at the app level.

## Auth

- `AuthService` — calls ASP.NET Identity cookie endpoints (`/login`, `/logout`)
- `AuthGuard` — redirects to `/signin` if not authenticated
- `AuthInterceptor` — intercepts 401 responses

## Media uploads in bone editors

`MediaUploadMap` in `src/app/core/` maps bone types to their upload endpoint config. Register new bone types there if they support media uploads.

## Build

```bash
npx nx build bonnie-cms-sample
npx nx lint bonnie-cms-sample
```
