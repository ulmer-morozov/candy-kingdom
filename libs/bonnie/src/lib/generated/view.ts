/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IHaveSkeleton } from "./i-have-skeleton";
import { IEquatable } from "./i-equatable";
import type { Bone } from "./bone";

export interface View {
	id: string;
	code: string;
	bones: Bone[];
}
