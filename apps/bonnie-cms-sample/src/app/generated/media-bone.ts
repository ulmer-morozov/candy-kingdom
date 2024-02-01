/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { Bone } from "@candy-kingdom/bonnie";
import { PixMediaUnion } from "@candy-kingdom/bonnie";
import { LocalizedString } from "@candy-kingdom/bonnie";

export interface MediaBone extends Bone {
    readonly boneType: string;
    media: PixMediaUnion;
    title: LocalizedString;
    text: LocalizedString;
    alt: LocalizedString;
    link: LocalizedString;
}
