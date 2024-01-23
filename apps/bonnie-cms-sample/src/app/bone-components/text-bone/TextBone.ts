import { Bone, LocalizedString } from '@candy-kingdom/bonnie';

export interface TextBone extends Bone {
    title: LocalizedString;
    text: LocalizedString;
}
