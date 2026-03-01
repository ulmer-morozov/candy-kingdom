/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { PixMedia } from "./pix-media";
import { IEquatable } from "./i-equatable";
import { VideoSource } from "./video-source";

export interface Video extends PixMedia {
	readonly $type: "video";
	sources: VideoSource[];
	type: "video";
}
