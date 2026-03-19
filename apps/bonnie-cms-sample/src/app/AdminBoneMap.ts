import type { Type } from "@angular/core";

import type { BoneEditorMap, IBoneEditor } from "@candy-kingdom/bonnie-cms";

import { MediaBoneEditorComponent } from "./bone-editors";
import { PageListBoneEditorComponent } from "./bone-editors/page-list-bone-editor/page-list-bone-editor.component";
import { TextBoneEditorComponent } from "./bone-editors/text-bone-editor/text-bone-editor.component";

const map = new Map<string, Type<IBoneEditor<any>>>();

map.set("text", TextBoneEditorComponent);
map.set("media", MediaBoneEditorComponent);
map.set("page-list", PageListBoneEditorComponent);

export const AdminBoneMap: BoneEditorMap = map;
