/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { PageData } from "./page-data";
import { PageBase } from "./page-base";
import { IHaveSkeleton } from "./i-have-skeleton";
import { IEquatable } from "./i-equatable";
import { IPage } from "./i-page";

export interface Page<T extends PageData> extends PageBase {
    data: T;
}
