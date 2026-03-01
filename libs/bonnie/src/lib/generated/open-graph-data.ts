/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { FileSrc } from "./file-src";
import { IEquatable } from "./i-equatable";
import type { ImageMeta } from "./image-meta";
import type { LocalizedObject } from "./localized-object";
import type { LocalizedString } from "./localized-string";

export interface OpenGraphData {
	title: LocalizedString;
	description: LocalizedString;
	image: LocalizedObject<FileSrc<ImageMeta>>;
}
