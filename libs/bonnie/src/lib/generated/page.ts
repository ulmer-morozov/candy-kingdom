/**
 * This is a TypeGen auto-generated file.
 * Any changes made to this file can be lost when this file is regenerated.
 */

import type { PageBase } from "./page-base";
import type { PageData } from "./page-data";

export interface Page<T extends PageData> extends PageBase {
	data: T;
}
