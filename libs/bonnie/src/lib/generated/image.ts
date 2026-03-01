/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { PixMedia } from "./pix-media";
import { IEquatable } from "./i-equatable";
import type { ImageSource } from "./image-source";

export interface Image extends PixMedia {
	readonly $type: "image";
	sources: ImageSource[];
	type: "image";
}
