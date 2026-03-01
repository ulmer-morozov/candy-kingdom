/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IEquatable } from "./i-equatable";
import type { ImageMeta } from "./image-meta";
import type { MediaSource } from "./media-source";
import { MediaSourceBase } from "./media-source-base";
import type { SizesItem } from "./sizes-item";

export interface ImageSource extends MediaSource<ImageMeta> {
	sizes: SizesItem[];
}
