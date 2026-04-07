# @candy-kingdom/bonnie-cms

Angular CMS admin UI library for CandyKingdom. Provides all components, services, base classes, and directives needed to build a content management interface on top of `@candy-kingdom/bonnie`.

## Installation

```bash
npm install @candy-kingdom/bonnie-cms
```

Peer dependency: `@candy-kingdom/bonnie`

## What's included

### Skeleton editor

The skeleton editor lets content editors add, remove, reorder, and configure bones on a page.

```html
<bonc-skeleton-editor
  [map]="adminBoneMap"
  [templates]="boneTemplates"
  [locale]="activeLocale"
  [device]="activeDevice"
  [(ngModel)]="page.bones"
/>
```

- `[map]` — a `BoneEditorMap` mapping bone type strings to editor component classes
- `[templates]` — `IBoneTemplate[]` listing the bone types that can be added
- `[(ngModel)]` — two-way binding via `ControlValueAccessor`

### Bone editors

Create a bone editor by extending `BoneEditorBaseComponent<TBone>` and implementing `IBoneEditor<TBone>`:

```typescript
import { BoneEditorBaseComponent, ContentPreset, createPreset } from '@candy-kingdom/bonnie-cms';

@Component({ /* ... */ })
export class TextBoneEditorComponent extends BoneEditorBaseComponent<TextBone> {
  getPresets(): ContentPreset<TextBone>[] {
    return [
      createPreset({ title: 'Default', style: TextBoneStyle.default }),
      createPreset({ title: 'Decorated', style: TextBoneStyle.decorated }),
    ];
  }
}
```

Access the bone value as `this.bone()` in TypeScript and `bone()` in templates. The base class manages dirty state, preset cycling, and save/cancel lifecycle.

### Data services

```typescript
import { DataService, AdminDataService, API_BASE_URL } from '@candy-kingdom/bonnie-cms';
```

Provide the API base URL:

```typescript
providers: [{ provide: API_BASE_URL, useValue: 'https://my-api.example.com' }]
```

- `DataService.getPage(route)` — fetches a page and resolves any `IHaveDataRoute` bones
- `DataService.getView(code)` — fetches a named view (e.g. navigation)
- `DataService.getSettings(ids[])` — fetches CMS settings
- `AdminDataService.storePage(page)` — saves a page via the admin API
- `AdminDataService.updateSettings(settings[])` — saves settings

### Form components

Ready-made form components for common content fields:

| Component | Selector | Purpose |
| --- | --- | --- |
| `TextFormComponent` | `<bonc-text-form>` | Plain text field |
| `TranslationFormComponent` | `<bonc-translation-form>` | Localized text (all locales) |
| `SeoFormComponent` | `<bonc-seo-form>` | OpenGraph / SEO metadata |
| `OneImageFormComponent` | `<bonc-one-image-form>` | Single image upload |
| `SvgFormComponent` | `<bonc-svg-form>` | SVG upload |
| `FileFormComponent` | `<bonc-file-form>` | Generic file upload |

### Upload components

```html
<bonc-media-uploader (uploaded)="onImage($event)" />
<bonc-file-uploader (uploaded)="onFile($event)" />
```

### Admin controls

```html
<bonc-admin-controls [(locale)]="locale" [(device)]="device" />
```

Toolbar for switching the active locale (`en`, `ru`) and device preview (`Desktop`, `Tablet`, `Mobile`).

### EditableDirective

The fundamental data binding primitive. Implements `ControlValueAccessor` and manages an edit/save/cancel lifecycle with dirty tracking.

```html
<div [boncEditable]="value" (saved)="onSave($event)">
  <!-- edit UI here -->
</div>
```

## Patterns

### BoneEditorMap

```typescript
import { BoneEditorMap } from '@candy-kingdom/bonnie-cms';

export const AdminBoneMap: BoneEditorMap = new Map([
  ['text', TextBoneEditorComponent],
  ['media', MediaBoneEditorComponent],
]);
```

### Generated types

TypeScript interfaces in `src/lib/generated/` are auto-generated from C# source in `libs/marcy-cms`. Do not edit them manually.

Key interfaces: `Setting`, `SettingGroup`, `SettingData`, `TextSettingData`, `LocalizedTextSettingData`, `OneImageSettingData`, `SvgSettingData`, `FileSettingData`.

## Build

```bash
npx nx build bonnie-cms
```
