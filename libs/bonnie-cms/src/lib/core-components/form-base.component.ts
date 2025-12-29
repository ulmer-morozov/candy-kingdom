import { Component, Input, inject } from '@angular/core';

import { EditableDirective } from './editable.directive';

@Component({
  template: ''
})
export abstract class FormBaseComponent<TData = unknown> {
  public static readonly inputs = ['locale'];

  protected name = '';
  private _locale = '';

  public readonly editable = inject(EditableDirective<TData>, { host: true });

  @Input()
  public set locale(value: string) {
    this._locale = value;
  }

  public get locale(): string {
    return this._locale;
  }
}
