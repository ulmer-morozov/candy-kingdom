import { DestroyRef, Directive, effect, inject, output, signal } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";

import { fromEvent, merge, NEVER, type Observable, Subject, takeUntil } from "rxjs";

import type * as M_CORE from "../generated";
import * as utils from "./utils";

@Directive({
	standalone: true,
	selector: "[bonSrcBase]",
})
export class SrcBaseDirective<T extends M_CORE.Image | M_CORE.Video> {
	public readonly ratioChange = output<number>();
	public readonly srcChange = output<T | undefined>();

	private readonly _ratio = signal<number>(0);
	public readonly ratio = this._ratio.asReadonly();

	private readonly _queryChangeClearSubject = new Subject<void>();

	public readonly data = signal<T | undefined>(undefined);

	private readonly _destroyRef = inject(DestroyRef);

	constructor() {
		effect(() => {
			const ratio = this._ratio();
			this.ratioChange.emit(ratio);
		});

		effect(() => {
			const val = this.data();
			this.onDataChange(val);
		});
	}

	private onDataChange(val: T | undefined): void {
		if (val !== undefined && val !== null && val.sources.length === 0) {
			console.warn(`image should have sources!`);
			this.data.set(undefined);
			return;
		}

		// clear mediaQuery subscriptions
		this._queryChangeClearSubject.next();

		// ratio
		this._ratio.set(0);

		this.srcChange.emit(val);

		if (val === undefined || val.sources.length === 0) {
			return;
		}

		const allRatios = val.sources
			.flatMap((x) => x.srcSet)
			.map((x) => x.meta.ratio)
			.filter(utils.distinct);

		if (allRatios.length === 1) {
			// same ratio for all
			this._ratio.set(allRatios[0]);
			return;
		}

		this.watchMediaQueries().subscribe(() => {
			this.calcRatio();
			console.log("watchMediaQueries calcRatio");
		});

		this.calcRatio();
	}

	private calcRatio(): void {
		const data = this.data();
		if (data === undefined || data === null || data.sources.length === 0) return;

		for (let i = 0; i < data.sources.length; i++) {
			const source = data.sources[i];
			const srcRatios = source.srcSet
				.sort(utils.descendingT((x) => x.meta.width))
				.map((x) => x.meta.ratio)
				.filter(utils.distinct);

			if (srcRatios.length === 0) {
				return;
			}

			if (srcRatios.length > 1) {
				console.warn(
					`each source should have srcSet with same ratio. founded: ${srcRatios.join(", ")}`,
				);
			}

			const ratio = srcRatios[0]; // most accurate ratio in biggest image

			if (source.mediaQuery.length === 0) {
				this._ratio.set(ratio);
				return;
			}

			if (typeof window === "undefined" || typeof window.matchMedia === "undefined") return;

			const mediaQueryList = window.matchMedia(source.mediaQuery);

			if (mediaQueryList.matches) {
				this._ratio.set(ratio);
				return;
			}
		}
	}

	public watchMediaQueries(): Observable<MediaQueryListEvent> {
		console.log("watchMediaQueries");
		const data = this.data();

		if (
			data === undefined ||
			data === null ||
			typeof window === "undefined" ||
			typeof window.matchMedia === "undefined"
		)
			return NEVER.pipe(
				takeUntilDestroyed(this._destroyRef),
				takeUntil(this._queryChangeClearSubject),
			);

		const mediaQueries = data.sources
			.map((x) => x.mediaQuery)
			.filter(utils.distinct)
			.filter((x) => x.length > 0);

		console.log("watchMediaQueries mediaQueries", mediaQueries);
		const queryObservables = mediaQueries.map((media) => {
			const queryList = window.matchMedia(media);
			const observable = fromEvent<MediaQueryListEvent>(queryList, "change").pipe(
				takeUntilDestroyed(this._destroyRef),
				takeUntil(this._queryChangeClearSubject),
			);

			return observable;
		});

		const combinedObservable = merge(...queryObservables);
		return combinedObservable;
	}
}
