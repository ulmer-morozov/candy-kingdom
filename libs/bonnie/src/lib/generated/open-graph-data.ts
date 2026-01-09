/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IEquatable } from "./i-equatable";
import { LocalizedString } from "./localized-string";
import { LocalizedObject } from "./localized-object";
import { FileSrc } from "./file-src";
import { ImageMeta } from "./image-meta";

export interface OpenGraphData {
    title: LocalizedString;
    description: LocalizedString;
    image: LocalizedObject<FileSrc<ImageMeta>>;
}
