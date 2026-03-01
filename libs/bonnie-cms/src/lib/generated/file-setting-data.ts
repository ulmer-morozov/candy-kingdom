/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { SettingData } from "./setting-data";
import { IEquatable } from "./i-equatable";
import type { FileSrcFileMeta } from "@candy-kingdom/bonnie";

export interface FileSettingData extends SettingData {
	allowedMimeTypes: string[];
	src: FileSrcFileMeta;
}
