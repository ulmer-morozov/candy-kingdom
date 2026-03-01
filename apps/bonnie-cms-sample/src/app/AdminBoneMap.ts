import { BoneEditorMap, IBoneEditor } from "@candy-kingdom/bonnie-cms";
import { TextBoneEditorComponent } from "./bone-editors/text-bone-editor/text-bone-editor.component";
import { PageListBoneEditorComponent } from "./bone-editors/page-list-bone-editor/page-list-bone-editor.component";
import { Type } from "@angular/core";
import { MediaBoneEditorComponent } from "./bone-editors";

const map = new Map<string, Type<IBoneEditor<any>>>();

map.set("text", TextBoneEditorComponent);
map.set("media", MediaBoneEditorComponent);
map.set("page-list", PageListBoneEditorComponent);

export const AdminBoneMap: BoneEditorMap = map;
