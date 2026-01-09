/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { Bone } from "@candy-kingdom/bonnie";
import { LocalizedString } from "@candy-kingdom/bonnie";

export interface TextBone extends Bone {
    readonly $type: 'text';
    title: LocalizedString;
    text: LocalizedString;
    type: 'text';
}
