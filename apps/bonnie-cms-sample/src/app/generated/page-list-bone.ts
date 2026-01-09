/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { Bone } from "@candy-kingdom/bonnie";
import { LocalizedString } from "@candy-kingdom/bonnie";
import { PageBase } from "@candy-kingdom/bonnie";

export interface PageListBone extends Bone {
    readonly $type: 'page-list';
    title: LocalizedString;
    dataRoute: string;
    data: PageBase[];
    type: 'page-list';
}
