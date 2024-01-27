/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import { SettingData } from "./setting-data";
import { IEquatable } from "./i-equatable";
import { TextSettingType } from "./text-setting-type";
import { LocalizedString } from "@candy-kingdom/bonnie";

export interface LocalizedTextSettingData extends SettingData {
    text: LocalizedString;
    textType: TextSettingType;
}
