/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { FileFormat, FileSrcImageMeta } from "@candy-kingdom/bonnie";

import type { SettingData } from "./setting-data";

export interface OneImageSettingData extends SettingData {
	width: number;
	height: number;
	format: FileFormat;
	allowedMimeTypes: string[];
	src: FileSrcImageMeta;
}
