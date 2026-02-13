import { ElementRef, OnDestroy, Component, ChangeDetectorRef, inject, signal, effect, input, output } from '@angular/core';
import { UnsubscriberService } from './unsubscribe.service';

@Component({
    selector: 'bon-intersection',
    standalone: true,
    template: '<ng-content></ng-content>',
    styles: [':host{display:block}'],
    providers: [UnsubscriberService]
})
export class IntersectionComponent implements OnDestroy {
    public readonly intersected = output<void>();

    public readonly session = input<any>();

    private readonly _hostRef = inject(ElementRef);
    private readonly _u = inject(UnsubscriberService);
    private readonly _cd = inject(ChangeDetectorRef);

    private readonly intersectionObserver?: IntersectionObserver;
    private readonly _intersected = signal<boolean>(false);

    public readonly intersectedOnce = this._intersected.asReadonly();

    private _session: any; // todo: add type

    constructor() {
        this._cd.detach();

        if (typeof window === 'undefined' || typeof IntersectionObserver === 'undefined') {
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
            if (this._session === newSession)
                return;

            console.log('reset intersection Observer');
            this._session = newSession;
            this.reset();
        });
    }

    private reset(): void {
        if (this.intersectionObserver === undefined || this.intersectionObserver === null)
            return;

        this.intersectionObserver.unobserve(this._hostRef.nativeElement);

        this._intersected.set(false);

        this.intersectionObserver.observe(this._hostRef.nativeElement);
    }

    private onIntersection(entries: IntersectionObserverEntry[], observer: IntersectionObserver): void {
        if (entries.length > 1) {
            console.warn('multi entries!');
        }

        const isIntersecting = entries[0].isIntersecting
        this._intersected.set(isIntersecting);

        console.log(`intersected ${isIntersecting}`, this._hostRef.nativeElement);

        // only once will recieve intersection
        if (isIntersecting) {
            observer.unobserve(this._hostRef.nativeElement);
        }
    }

    public ngOnDestroy(): void {
        this.intersectionObserver?.disconnect();
    }
}
