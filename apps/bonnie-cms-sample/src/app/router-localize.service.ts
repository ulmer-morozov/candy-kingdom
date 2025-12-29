import { Injectable, inject, signal, effect, Signal } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { LocalizeServiceBase } from '@candy-kingdom/bonnie';

@Injectable()
export class RouterLocalizeService extends LocalizeServiceBase {
  private readonly _locale = signal<string>('');
  public readonly locale: Signal<string>;
  private readonly route = inject(ActivatedRoute);

  constructor() {
    super();

    // Expose signal as read-only
    this.locale = this._locale.asReadonly();

    console.log(`new locale ${this._locale()}`);

    // Watch locale changes for logging and emit to subject
    effect(() => {
      console.log(`new locale ${this._locale()}`);
    });

    const next = (newLocale?: string) => {
      const defaultLocale = 'en';

      newLocale = newLocale?.toLowerCase() ?? defaultLocale;

      if (newLocale.length === 0 || this._locale() === newLocale)
        return;

      this._locale.set(newLocale);
    }

    this.route.params.subscribe
      (
        (param: Params) => next(param['locale'] as string)
      );

    this.route.data.subscribe
      (
        (param: Params) => next(param['locale'] as string)
      );
  }
}
