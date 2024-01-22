import { ElementRef, OnInit, OnDestroy, Component, Output, EventEmitter, Input, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject, Observable, filter, skip } from 'rxjs';
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

    private readonly intersectionObserver?: IntersectionObserver;
    private readonly _intersectSubject = new BehaviorSubject<boolean>(false);

    private _session: any;

    constructor(private readonly _hostRef: ElementRef, private readonly _u: UnsubscriberService, cd: ChangeDetectorRef) {
        cd.detach();
        // no template with variables, so we don't need to call changeDetection

        if (typeof window === 'undefined' || typeof IntersectionObserver === 'undefined') {
            // call intersection without any
            return;
        }

        this.intersectionObserver = new IntersectionObserver(this.onIntersection.bind(this));
    }

    public ngOnInit(): void {
        this._intersectSubject
            .pipe(this._u.takeUntilDestroy, filter(x => x === true))
            .subscribe(x => this.intersected.next());
    }

    @Input()
    public set session(newSession: any) {
        if (this._session === newSession)
            return;

        console.log('reset intersection Observer');

        this._session = newSession;
        this.reset();
    }

    public get intersectedOnce(): boolean {
        return this._intersectSubject.value;
    }

    private reset(): void {
        if (this.intersectionObserver === undefined || this.intersectionObserver === null)
            return;

        this.intersectionObserver.unobserve(this._hostRef.nativeElement);

        this._intersectSubject.next(false);

        this.intersectionObserver.observe(this._hostRef.nativeElement);
    }

    private onIntersection(entries: IntersectionObserverEntry[], observer: IntersectionObserver): void {
        if (entries.length > 1) {
            console.warn('multi entries!');
        }

        const isIntersecting = entries[0].isIntersecting
        this._intersectSubject.next(isIntersecting);

        console.log(`intersected ${isIntersecting}`, this._hostRef.nativeElement);

        // only once will recieve intersection
        if (isIntersecting) {
            observer.unobserve(this._hostRef.nativeElement);
        }
    }

    public ngOnDestroy(): void {
        this._intersectSubject.complete();
        this.intersectionObserver?.disconnect();
    }
}
