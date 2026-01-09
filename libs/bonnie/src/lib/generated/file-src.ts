/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { FileMeta } from "./file-meta";
import { FileSrcBase } from "./file-src-base";
import { IEquatable } from "./i-equatable";

export interface FileSrc<T extends FileMeta> extends FileSrcBase {
    meta: T;
}
