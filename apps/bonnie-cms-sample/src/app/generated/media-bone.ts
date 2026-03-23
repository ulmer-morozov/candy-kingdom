/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { Bone, LocalizedString, PixMediaUnion } from "@candy-kingdom/bonnie";

export interface MediaBone extends Bone {
	readonly $type: "media";
	media: PixMediaUnion;
	title: LocalizedString;
	text: LocalizedString;
	alt: LocalizedString;
	link: LocalizedString;
	type: "media";
}
