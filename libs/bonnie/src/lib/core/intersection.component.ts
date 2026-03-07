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

	private readonly intersectionObserver?: IntersectionObserver;
	private readonly _intersected = signal<boolean>(false);

	public readonly intersectedOnce = this._intersected.asReadonly();

	private _session: unknown; // todo: remove

	constructor() {
		if (typeof window === "undefined" || typeof IntersectionObserver === "undefined") {
			return;
		}

		this.intersectionObserver = new IntersectionObserver(this.onIntersection.bind(this));

		effect(() => {
			if (this._intersected()) {
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

		inject(DestroyRef).onDestroy(() => this.intersectionObserver?.disconnect());
	}

	private reset(): void {
		if (this.intersectionObserver === undefined || this.intersectionObserver === null) return;

		this.intersectionObserver.unobserve(this._hostRef.nativeElement);

		this._intersected.set(false);

		this.intersectionObserver.observe(this._hostRef.nativeElement);
	}

	private onIntersection(
		entries: IntersectionObserverEntry[],
		observer: IntersectionObserver,
	): void {
    if (entries.length > 1) {
			console.warn("multi entries!");
		}

		const isIntersecting = entries[0].isIntersecting;
		this._intersected.set(isIntersecting);

		console.log(`intersected ${isIntersecting}`, this._hostRef.nativeElement);

		// only once will recieve intersection
		if (isIntersecting) {
			observer.unobserve(this._hostRef.nativeElement);
		}
	}
}
