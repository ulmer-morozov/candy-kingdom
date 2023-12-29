/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { MediaSource } from "./media-source";
import { ImageMeta } from "./image-meta";
import { IEquatable } from "./i-equatable";
import { MediaSourceBase } from "./media-source-base";
import { SizesItem } from "./sizes-item";

export interface ImageSource extends MediaSource<ImageMeta> {
    mediaQuery: string;
    sizes: SizesItem[];
}
