/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { FileMeta } from "./file-meta";
import type { FileSrc } from "./file-src";
import { IEquatable } from "./i-equatable";
import type { MediaSourceBase } from "./media-source-base";

export interface MediaSource<TMeta extends FileMeta> extends MediaSourceBase {
	srcSet: FileSrc<TMeta>[];
	mediaQuery: string;
}
