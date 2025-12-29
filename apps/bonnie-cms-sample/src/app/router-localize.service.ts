import { Injectable, inject } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { LocalizeServiceBase } from '@candy-kingdom/bonnie';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class RouterLocalizeService extends LocalizeServiceBase {
  private readonly localeSubject: BehaviorSubject<string>;
  public readonly locale$: Observable<string>;
  private readonly route = inject(ActivatedRoute);

  constructor() {
    super();

    this.localeSubject = new BehaviorSubject('');
    this.locale$ = this.localeSubject.asObservable();

    this.localeSubject.subscribe(x => {
      console.log(`new locale ${x}`);
    });

    const next = (newLocale?: string) => {
      const defaultLocale = 'en';

      newLocale = newLocale?.toLowerCase() ?? defaultLocale;

      if (newLocale.length === 0 || this.localeSubject.value === newLocale)
        return;

      this.localeSubject.next(newLocale);
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

  public override get locale(): string {
    return this.localeSubject.value;
  }
}
