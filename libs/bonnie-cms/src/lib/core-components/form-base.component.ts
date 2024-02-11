import { Component, Host, Input } from '@angular/core';

import { EditableDirective } from './editable.directive';

@Component({
  template: ''
})
export abstract class FormBaseComponent<TData = unknown> {
  public static readonly inputs = ['locale'];

  protected name = '';
  private _locale = '';

  constructor(@Host() public readonly editable: EditableDirective<TData>) { }

  @Input()
  public set locale(value: string) {
    this._locale = value;
  }

  public get locale(): string {
    return this._locale;
  }
}
