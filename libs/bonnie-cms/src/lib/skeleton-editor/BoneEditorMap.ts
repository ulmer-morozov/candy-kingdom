import type { Type } from "@angular/core";

import type { IBoneEditor } from "./IBoneEditor";

export type BoneEditorMap = ReadonlyMap<string, Type<IBoneEditor>>;
