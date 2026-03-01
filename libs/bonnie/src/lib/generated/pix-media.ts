/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IEquatable } from "./i-equatable";
import type { MediaSourceBase } from "./media-source-base";

export interface PixMedia {
	sources: MediaSourceBase[];
	type: "image" | "video";
}
