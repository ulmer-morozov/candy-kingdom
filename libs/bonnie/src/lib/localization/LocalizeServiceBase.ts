import type { Signal } from "@angular/core";
import type { LocalizedObject, LocalizedString } from "../generated";
import { isLocalUrlString } from "../core/utils";

export abstract class LocalizeServiceBase {
	public abstract get locale(): Signal<string>;

	public getLocalized<T>(value: LocalizedObject<T>, defaultValue: T, locale?: string): T {
		if (value === undefined) return defaultValue;

		locale = locale ?? this.locale();

		const obj = value[locale] ?? defaultValue;
		return obj;
	}

	public getLocalizedText(value: LocalizedString, locale?: string): string {
		return this.getLocalized<string>(value, "", locale);
	}

	public isLocalUrl(value: LocalizedString, locale?: string): boolean {
		const url = this.getLocalizedText(value, locale);
		return isLocalUrlString(url);
	}
}
