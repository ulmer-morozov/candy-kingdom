/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { LocalizedString } from "@candy-kingdom/bonnie";

import { IEquatable } from "./i-equatable";
import type { SettingData } from "./setting-data";
import type { TextSettingType } from "./text-setting-type";

export interface LocalizedTextSettingData extends SettingData {
	text: LocalizedString;
	textType: TextSettingType;
}
