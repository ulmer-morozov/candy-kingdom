/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { IEquatable } from "./i-equatable";
import { ISetting } from "./i-setting";
import type { SettingBase } from "./setting-base";
import type { SettingData } from "./setting-data";

export interface Setting<T extends SettingData> extends SettingBase {
	data: T;
}
