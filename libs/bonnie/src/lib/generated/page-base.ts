/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IHaveSkeleton } from "./i-have-skeleton";
import { IEquatable } from "./i-equatable";
import type { PublishStatus } from "./publish-status";
import type { LocalizedString } from "./localized-string";
import type { OpenGraphData } from "./open-graph-data";
import type { Bone } from "./bone";

export interface PageBase {
	id: string;
	url: string;
	route: string;
	order: number;
	publishStatus: PublishStatus;
	title: LocalizedString;
	openGraph: OpenGraphData;
	bones: Bone[];
	children: PageBase[];
}
