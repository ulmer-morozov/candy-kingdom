/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { FileMeta } from "./file-meta";
import type { FileSrcBase } from "./file-src-base";

export interface FileSrc<T extends FileMeta> extends FileSrcBase {
	meta: T;
}
