/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { SettingData } from "./setting-data";
import type { SettingBase } from "./setting-base";
import { IEquatable } from "./i-equatable";
import { ISetting } from "./i-setting";

export interface Setting<T extends SettingData> extends SettingBase {
	data: T;
}
