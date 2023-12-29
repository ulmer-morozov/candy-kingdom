/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { PixMedia } from "./pix-media";
import { IEquatable } from "./i-equatable";
import { ImageSource } from "./image-source";

export interface Image extends PixMedia {
    sources: ImageSource[];
    type: 'image';
}
