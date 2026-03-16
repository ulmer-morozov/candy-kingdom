import {
	Component,
	DestroyRef,
	ElementRef,
	effect,
	inject,
	input,
	output,
	signal,
} from "@angular/core";

@Component({
	selector: "bon-intersection",
	template: "<ng-content></ng-content>",
	styles: [":host{display:block}"],
})
export class IntersectionComponent {
	public readonly intersected = output<void>();

	public readonly session = input<unknown>();

	private readonly _hostRef = inject(ElementRef);

	private readonly _intersectionObserver?: IntersectionObserver;
	private readonly _isIntersected = signal<boolean>(false);

	public readonly isIntersected = this._isIntersected.asReadonly();

	private _session: unknown; // todo: remove

	constructor() {
		if (typeof window === "undefined" || typeof IntersectionObserver === "undefined") {
			return;
		}

		this._intersectionObserver = new IntersectionObserver(this.onIntersection.bind(this));

		effect(() => {
			if (this._isIntersected()) {
				this.intersected.emit();
			}
		});

		effect(() => {
			const newSession = this.session();

			if (this._session === newSession) return;

			console.log("reset intersection Observer");
			this._session = newSession;
			this.reset();
		});

		inject(DestroyRef).onDestroy(() => this._intersectionObserver?.disconnect());
	}

	private reset(): void {
		if (this._intersectionObserver === undefined || this._intersectionObserver === null) return;

		this._intersectionObserver.unobserve(this._hostRef.nativeElement);

		this._isIntersected.set(false);

		this._intersectionObserver.observe(this._hostRef.nativeElement);
	}

	private onIntersection(
		entries: IntersectionObserverEntry[],
		observer: IntersectionObserver,
	): void {
		if (entries.length > 1) {
			console.warn("multi entries!");
		}

		const isIntersecting = entries[0].isIntersecting;
		this._isIntersected.set(isIntersecting);

		console.log(`intersected ${isIntersecting}`, this._hostRef.nativeElement);

		// only once will recieve intersection
		if (isIntersecting) {
			observer.unobserve(this._hostRef.nativeElement);
		}
	}
}
