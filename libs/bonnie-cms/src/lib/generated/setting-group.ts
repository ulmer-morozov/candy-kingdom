/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IEquatable } from "./i-equatable";
import type { Setting } from "./setting";

export interface SettingGroup {
	id: string;
	order: number;
	title: string;
	records: Setting<any>[];
}
