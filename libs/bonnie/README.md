# @candy-kingdom/bonnie

Core Angular rendering library for CandyKingdom CMS. Provides all components, directives, pipes, and TypeScript interfaces needed to display content pages served from the Marcy API.

## Installation

```bash
npm install @candy-kingdom/bonnie
```

## What's included

### Skeleton system

The skeleton system dynamically renders a list of `Bone` objects into Angular components. You provide a `BoneMap` that maps bone type strings to component classes; `SkeletonComponent` instantiates the right component for each bone.

```typescript
import { SkeletonComponent, BoneMap } from '@candy-kingdom/bonnie';

const boneMap = new BoneMap([
  ['text', TextBoneComponent],
  ['media', MediaBoneComponent],
]);
```

```html
<bon-skeleton [map]="boneMap" [bones]="page.bones" />
```

### Media components

Responsive image and video rendering backed by Marcy's processing pipeline.

```html
<!-- Unified dispatcher — picks image or video based on src.type -->
<bon-media [src]="bone().media" />

<!-- Image with responsive <picture> element -->
<bon-image />  <!-- requires [imgsrc] directive on a parent -->

<!-- Video element -->
<bon-video />  <!-- requires [vidsrc] directive on a parent -->
```

### Localization

```typescript
import { LocalizeServiceBase, LocalizePipe } from '@candy-kingdom/bonnie';
```

- `LocalizePipe` / `LocalizeObjectPipe` — transform `LocalizedString` / `LocalizedObject<T>` to the active locale's value
- `LocalizationIsEmptyPipe` / `LocalizationIsNotEmptyPipe` — check emptiness across all locales
- `LocalizeUrlPipe` / `IsLocalUrlPipe` / `IsNotLocalUrlPipe` — URL helpers

Implement `LocalizeServiceBase` to provide a `locale: Signal<string>` for the active locale.

### Core types

All TypeScript interfaces are generated from C# source in `libs/marcy` by `apps/marcy-gen`. Do not edit files in `src/lib/generated/` manually.

Key interfaces: `Bone`, `Page`, `PageBase`, `View`, `Image`, `Video`, `PixMedia`, `ImageSource`, `VideoSource`, `FileSrc`, `LocalizedString`, `LocalizedObject`, `IHaveDataRoute`, `IHaveDataRouteWithData`, `PageData`.

## Patterns

### Signals

This library uses Angular signals exclusively. Access signal values by calling them:

```typescript
// In templates
{{ bone().title | localize }}

// In TypeScript
const text = this.bone().text;
```

### Bone components

Bone components receive their data through `BoneDirective`. The directive exposes a `bone` model signal:

```typescript
import { BoneDirective, IBoneComponent } from '@candy-kingdom/bonnie';

@Component({ hostDirectives: [BoneDirective] })
export class MyBoneComponent implements IBoneComponent {
  bd = inject(BoneDirective<MyBone>);
  // access data as this.bd.bone()
}
```

### SSR compatibility

All media components guard against `typeof window === 'undefined'`. The library is fully compatible with Angular SSR.

## Build

```bash
npx nx build bonnie
```
