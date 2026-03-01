import { Injectable, inject, signal, effect } from "@angular/core";
import { ActivatedRoute, Params } from "@angular/router";
import { LocalizeServiceBase } from "@candy-kingdom/bonnie";

@Injectable()
export class RouterLocalizeService extends LocalizeServiceBase {
	public readonly locale = signal<string>("");

	private readonly route = inject(ActivatedRoute);

	constructor() {
		super();

		console.log(`new locale ${this.locale()}`);

		// Watch locale changes for logging and emit to subject
		effect(() => {
			console.log(`new locale ${this.locale()}`);
		});

		const next = (newLocale?: string) => {
			const defaultLocale = "en";

			newLocale = newLocale?.toLowerCase() ?? defaultLocale;

			if (newLocale.length === 0 || this.locale() === newLocale) return;

			this.locale.set(newLocale);
		};

		this.route.params.subscribe((param: Params) => next(param["locale"] as string));

		this.route.data.subscribe((param: Params) => next(param["locale"] as string));
	}
}
