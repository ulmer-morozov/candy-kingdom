import { ElementRef, OnInit, OnDestroy, Component, Output, EventEmitter, Input, ChangeDetectorRef, inject, signal, effect, EffectRef, Signal } from '@angular/core';
import { UnsubscriberService } from './unsubscribe.service';

@Component({
    selector: 'bon-intersection',
    template: '<ng-content></ng-content>',
    styles: [':host{display:block}'],
    providers: [UnsubscriberService]
})
export class IntersectionComponent implements OnInit, OnDestroy {
    @Output()
    public readonly intersected = new EventEmitter<void>();

    private readonly _hostRef = inject(ElementRef);
    private readonly _u = inject(UnsubscriberService);
    private readonly _cd = inject(ChangeDetectorRef);

    private readonly intersectionObserver?: IntersectionObserver;
    private readonly _intersected = signal<boolean>(false);

    public readonly intersectedOnce = this._intersected.asReadonly();

    private _session: any;
    private readonly _effectCleanup?: EffectRef;

    constructor() {
        this._cd.detach();
        // no template with variables, so we don't need to call changeDetection

        if (typeof window === 'undefined' || typeof IntersectionObserver === 'undefined') {
            // call intersection without any
            return;
        }

        this.intersectionObserver = new IntersectionObserver(this.onIntersection.bind(this));

        // Watch for intersection changes and emit when it becomes true
        this._effectCleanup = effect(() => {
            if (this._intersected()) {
                this.intersected.next();
            }
        });
    }

    public ngOnInit(): void {
    }

    @Input()
    public set session(newSession: any) {
        if (this._session === newSession)
            return;

        console.log('reset intersection Observer');

        this._session = newSession;
        this.reset();
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
        this._effectCleanup?.destroy();
        this.intersectionObserver?.disconnect();
    }
}
