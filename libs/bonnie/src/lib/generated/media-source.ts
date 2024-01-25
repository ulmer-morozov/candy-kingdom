/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { FileMeta } from "./file-meta";
import { MediaSourceBase } from "./media-source-base";
import { IEquatable } from "./i-equatable";
import { FileSrc } from "./file-src";

export interface MediaSource<TMeta extends FileMeta> extends MediaSourceBase {
    srcSet: FileSrc<TMeta>[];
    mediaQuery: string;
}
