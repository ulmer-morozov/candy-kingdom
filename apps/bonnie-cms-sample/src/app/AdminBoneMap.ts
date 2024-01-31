import { BoneEditorMap } from '@candy-kingdom/bonnie-cms';
import { TextBoneEditorComponent } from './bone-editors/text-bone-editor/text-bone-editor.component';

const fullMap = new BoneEditorMap();

fullMap.set('text', TextBoneEditorComponent);

export const AdminBoneMap: Readonly<BoneEditorMap> = fullMap;
