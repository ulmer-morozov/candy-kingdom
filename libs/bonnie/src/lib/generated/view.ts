/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { Bone } from "./bone";
import { IEquatable } from "./i-equatable";
import { IHaveSkeleton } from "./i-have-skeleton";

export interface View {
	id: string;
	code: string;
	bones: Bone[];
}
