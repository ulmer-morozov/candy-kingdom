import { inject, Pipe, type PipeTransform } from "@angular/core";
import { DomSanitizer, type SafeResourceUrl } from "@angular/platform-browser";

import type { LocalizedObject, LocalizedString } from "../generated";
import { LocalizeServiceBase } from "./LocalizeServiceBase";

@Pipe({ name: "localize", pure: false })
export class LocalizePipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);

	public transform(value: LocalizedString, locale?: string): string {
		if (value === undefined || value === null) return "";

		return this._localizeService.getLocalizedText(value, locale);
	}
}

@Pipe({ name: "localizeObject", pure: false })
export class LocalizeObjectPipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);

	public transform<T>(object: LocalizedObject<T>, locale?: string): T | undefined {
		if (object === undefined || object === null) return undefined;

		return this._localizeService.getLocalized<T | undefined>(object, undefined, locale);
	}
}

@Pipe({ name: "localizationIsNotEmpty", pure: false })
export class LocalizationIsNotEmptyPipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);

	public transform(value: LocalizedString, locale?: string): boolean {
		if (value === undefined || value === null) return false;

		const text = this._localizeService.getLocalizedText(value, locale);

		const isNotEmpty = text.trim().length > 0;
		return isNotEmpty;
	}
}

@Pipe({ name: "localizationIsEmpty", pure: false })
export class LocalizationIsEmptyPipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);

	public transform(value: LocalizedString, locale?: string): boolean {
		if (value === undefined || value === null) return true;

		const text = this._localizeService.getLocalizedText(value, locale);

		const isEmpty = text.trim().length === 0;
		return isEmpty;
	}
}

@Pipe({ name: "localizeUrl", pure: false })
export class LocalizeUrlPipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);
	private readonly _domSanitizer = inject(DomSanitizer);

	public transform(value: LocalizedString, locale?: string): SafeResourceUrl {
		if (value === undefined || value === null) return "";

		const urlString = this._localizeService.getLocalizedText(value, locale);
		const safeUrl = this._domSanitizer.bypassSecurityTrustResourceUrl(urlString);
		return safeUrl;
	}
}

@Pipe({ name: "isLocalUrl", pure: false })
export class IsLocalUrlPipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);

	public transform(value: LocalizedString, locale?: string): boolean {
		if (value === undefined || value === null) return false;

		const isLocal = this._localizeService.isLocalUrl(value, locale);
		return isLocal;
	}
}

@Pipe({ name: "isNotLocalUrl", pure: false })
export class IsNotLocalUrlPipe implements PipeTransform {
	private readonly _localizeService = inject(LocalizeServiceBase);

	public transform(value: LocalizedString, locale?: string): boolean {
		if (value === undefined || value === null) return false;

		const isLocal = this._localizeService.isLocalUrl(value, locale);
		return !isLocal;
	}
}
