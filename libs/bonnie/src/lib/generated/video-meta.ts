/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { PixMeta } from "./pix-meta";
import { IEquatable } from "./i-equatable";
import { FileMeta } from "./file-meta";

export interface VideoMeta extends PixMeta {
    duration: number;
    fullFormat: string;
    hasAudio: boolean;
    frameRate: number;
}
