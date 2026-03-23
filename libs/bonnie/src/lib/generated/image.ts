/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { ImageSource } from "./image-source";
import type { PixMedia } from "./pix-media";

export interface Image extends PixMedia {
	readonly $type: "image";
	sources: ImageSource[];
	type: "image";
}
