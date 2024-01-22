// import { Directive, Injectable } from '@angular/core';
// import { ILocalizedString } from './ILocalizedString';
// import { ActivatedRoute, Params } from '@angular/router';

// import { BehaviorSubject } from 'rxjs';
// import { ILocalizedObject } from './ILocalizedObject';
// import { isLocalUrlString } from '../../../../../../../Old-code/Tuman/ClientApp/src/contracts/utils';
// import { LocalizeService } from './localize.service';
// import { Input } from '@angular/core';

// const localeQueryParamName = 'locale';
// const defaultLocale = 'en';

// @Directive({
//   selector: '[bon-localize]',
//   standalone: true
// })
// export class LocalizeDirective {
//   private readonly locale$: BehaviorSubject<string>;

//   private _forcedLocale = '';
//   private _routeLocale = '';

//   constructor(private readonly _localizeService: LocalizeService, route: ActivatedRoute) {
//     this.locale$ = new BehaviorSubject('');

//     route.params.subscribe
//       (
//         (param: Params) => {
//           this._routeLocale = param[localeQueryParamName] as string;


//           this.updateForcedLocale(localeFromParams);
//         }
//       );
//   }

//   @Input('locale')
//   public set forcedLocale(newValue: string | undefined) {
//     this._forcedLocale = newValue ?? '';
//   }

//   private updateForcedLocale = (forcedLocale: string) => {
//     if (forcedLocale === undefined || forcedLocale.trim() === '') {
//       this._forcedLocale = '';
//       this.updateCurentLocaleIfNeeded();
//       return;
//     }

//     this._forcedLocale = forcedLocale.toLowerCase();

//     this.updateCurentLocaleIfNeeded();

//     // console.log(`localeFromParams: ${forcedLocale} | systemLocale: ${systemLocale}`);
//   }

//   private updateCurentLocaleIfNeeded = (): void => {
//     const currentLocale = this.getCurrentLocale();

//     if (this.locale$.value !== currentLocale) {
//       this.locale$.next(currentLocale);
//     }
//   }

//   getCurrentLocale = (): string => {
//     const calculatedLocale = this._forcedLocale !== undefined && this._forcedLocale.length > 0
//       ? this._forcedLocale
//       : defaultLocale;

//     return calculatedLocale;
//   }

//   getLocalizedTextForLocale = (value: ILocalizedString, locale: string): string => this.getLocalizedForLocale<string>(value, locale, '');

//   getLocalizedText = (value: ILocalizedString): string => this.getLocalized<string>(value, '');

//   public getLocalized<T>(value: ILocalizedObject<T>, defaultValue: T): T {
//     const currentLocale = this.getCurrentLocale();
//     return this.getLocalizedForLocale(value, currentLocale, defaultValue);
//   }

//   public getLocalizedForLocale<T>(value: ILocalizedObject<T>, locale: string, defaultValue: T): T {
//     if (value === undefined)
//       return defaultValue;

//     const obj = value[locale] ?? defaultValue;
//     return obj;
//   }

//   public isLocalUrl(value: ILocalizedString, locale: string): boolean {
//     const url = this.getLocalizedTextForLocale(value, locale);
//     return isLocalUrlString(url);
//   }
// }
