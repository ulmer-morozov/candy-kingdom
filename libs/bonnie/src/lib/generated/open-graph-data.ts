/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IEquatable } from "./i-equatable";
import type { LocalizedString } from "./localized-string";
import type { LocalizedObject } from "./localized-object";
import type { FileSrc } from "./file-src";
import type { ImageMeta } from "./image-meta";

export interface OpenGraphData {
	title: LocalizedString;
	description: LocalizedString;
	image: LocalizedObject<FileSrc<ImageMeta>>;
}
