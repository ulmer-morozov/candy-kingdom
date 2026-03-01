import {
  DestroyRef,
  Directive,
  inject,
  signal,
  effect,
  output,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, fromEvent, NEVER, merge, Subject, takeUntil } from 'rxjs';

import * as MCore from '../generated';
import * as utils from './utils';

@Directive({
  standalone: true,
  selector: '[bonSrcBase]',
})
export class SrcBaseDirective<T extends MCore.Image | MCore.Video>{
  public readonly ratioChange = output<number>();
  public readonly srcChange = output<T | undefined>();

  private readonly _ratio = signal<number>(0);
  public readonly ratio = this._ratio.asReadonly();

  private readonly _queryChangeClearSubject = new Subject<void>();

  private _data?: T;

  private readonly _destroyRef = inject(DestroyRef);

  constructor() {

    effect(() => {
      const ratio = this._ratio();
      this.ratioChange.emit(ratio);
    });
  }

  public get data(): T | undefined {
    return this._data;
  }

  public set data(val: T | undefined) {
    console.log('set data', val);

    if (val !== undefined && val !== null && val.sources.length === 0) {
      console.warn(`image should have sources!`);
      val = undefined;
    }

    this._data = val;

    console.log('calling src change ', this._data);

    // clear mediaQuery subscriptions
    this._queryChangeClearSubject.next();

    // ratio
    this._ratio.set(0);

    this.srcChange.emit(this._data);

    if (this._data === undefined || this._data.sources.length === 0) {
      return;
    }

    const allRatios = this._data.sources
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
      console.log('watchMediaQueries calcRatio');
    });

    this.calcRatio();
  }

  private calcRatio(): void {
    if (
      this._data === undefined ||
      this._data === null ||
      this._data.sources.length === 0
    )
      return;

    for (let i = 0; i < this._data.sources.length; i++) {
      const source = this._data.sources[i];
      const srcRatios = source.srcSet
        .sort(utils.descendingT((x) => x.meta.width))
        .map((x) => x.meta.ratio)
        .filter(utils.distinct);

      if (srcRatios.length === 0) {
        // console.warn(`each source should have srcSet with same ratio. founded: ${srcRatios.join(', ')}`);
        return;
      }

      if (srcRatios.length > 1) {
        console.warn(
          `each source should have srcSet with same ratio. founded: ${srcRatios.join(
            ', '
          )}`
        );
      }

      const ratio = srcRatios[0]; // most accurate ratio in biggest image

      if (source.mediaQuery.length === 0) {
        this._ratio.set(ratio);
        return;
      }

      if (
        typeof window === 'undefined' ||
        typeof window.matchMedia === 'undefined'
      )
        return;

      const mediaQueryList = window.matchMedia(source.mediaQuery);

      if (mediaQueryList.matches) {
        this._ratio.set(ratio);
        return;
      }
    }
  }

  public watchMediaQueries(): Observable<MediaQueryListEvent> {
    console.log('watchMediaQueries');

    if (
      this._data === undefined ||
      this._data === null ||
      typeof window === 'undefined' ||
      typeof window.matchMedia === 'undefined'
    )
      return NEVER.pipe(
        takeUntilDestroyed(this._destroyRef),
        takeUntil(this._queryChangeClearSubject)
      );

    const mediaQueries = this._data.sources
      .map((x) => x.mediaQuery)
      .filter(utils.distinct)
      .filter((x) => x.length > 0);

    console.log('watchMediaQueries mediaQueries', mediaQueries);

    const queryObservables = mediaQueries.map((media) => {
      const queryList = window.matchMedia(media);
      const observable = fromEvent<MediaQueryListEvent>(
        queryList,
        'change'
      ).pipe(
        takeUntilDestroyed(this._destroyRef),
        takeUntil(this._queryChangeClearSubject)
      );

      return observable;
    });

    const combinedObservable = merge(...queryObservables);
    return combinedObservable;
  }

}
