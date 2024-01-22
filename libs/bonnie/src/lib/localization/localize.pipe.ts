import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { LocalizeServiceBase } from './LocalizeServiceBase';
import { LocalizedObject, LocalizedString } from '../generated';

@Pipe({ name: 'localize', pure: false })
export class LocalizePipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase) {
  }

  public transform(value: LocalizedString, locale?: string): string {
    if (value === undefined || value === null)
      return '';

    return this.localizeService.getLocalizedText(value, locale);
  }
}

@Pipe({ name: 'localizeObject', pure: false })
export class LocalizeObjectPipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase) { }

  public transform<T>(object: LocalizedObject<T>, locale?: string): T | undefined {
    if (object === undefined || object === null)
      return undefined;

    return this.localizeService.getLocalized<T | undefined>(object, undefined, locale);
  }
}

@Pipe({ name: 'localizationIsNotEmpty', pure: false })
export class LocalizationIsNotEmptyPipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase) { }

  public transform(value: LocalizedString, locale?: string): boolean {
    if (value === undefined || value === null)
      return false;

    const text = this.localizeService.getLocalizedText(value, locale);

    const isNotEmpty = text.trim().length > 0;
    return isNotEmpty;
  }
}

@Pipe({ name: 'localizationIsEmpty', pure: false })
export class LocalizationIsEmptyPipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase) { }

  public transform(value: LocalizedString, locale?: string): boolean {

    if (value === undefined || value === null)
      return true;

    const text = this.localizeService.getLocalizedText(value, locale);

    const isEmpty = text.trim().length === 0;
    return isEmpty;
  }
}


@Pipe({ name: 'localizeUrl', pure: false })
export class LocalizeUrlPipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase, private readonly domSanitizer: DomSanitizer) {

  }

  public transform(value: LocalizedString, locale?: string): SafeResourceUrl {
    if (value === undefined || value === null)
      return '';

    const urlString = this.localizeService.getLocalizedText(value, locale);
    const safeUrl = this.domSanitizer.bypassSecurityTrustResourceUrl(urlString);
    return safeUrl;
  }
}

@Pipe({ name: 'isLocalUrl', pure: false })
export class IsLocalUrlPipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase) { }

  public transform(value: LocalizedString, locale?: string): boolean {

    if (value === undefined || value === null)
      return false;

    const isLocal = this.localizeService.isLocalUrl(value, locale);
    return isLocal;
  }
}

@Pipe({ name: 'isNotLocalUrl', pure: false })
export class IsNotLocalUrlPipe implements PipeTransform {
  constructor(private readonly localizeService: LocalizeServiceBase) { }

  public transform(value: LocalizedString, locale?: string): boolean {

    if (value === undefined || value === null)
      return false;

    const isLocal = this.localizeService.isLocalUrl(value, locale);
    return !isLocal;
  }
}
