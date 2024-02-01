import { BoneEditorMap } from '@candy-kingdom/bonnie-cms';
import { TextBoneEditorComponent } from './bone-editors/text-bone-editor/text-bone-editor.component';
import { PageListBoneEditorComponent } from './bone-editors/page-list-bone-editor/page-list-bone-editor.component';

const fullMap = new BoneEditorMap();

fullMap.set('text', TextBoneEditorComponent);
fullMap.set('page-list', PageListBoneEditorComponent);

export const AdminBoneMap: Readonly<BoneEditorMap> = fullMap;
