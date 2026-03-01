import type { Type } from "@angular/core";
import type { IBoneComponent } from "./IBoneComponent";

export class BoneMap extends Map<string, Type<any>> {
	public getRequired(key: string): Type<IBoneComponent> {
		const value = this.get(key);

		if (value === undefined || value === null) {
			throw Error(`BoneMap doesn't contain type for key = ${key}`);
		}

		return value;
	}
}
