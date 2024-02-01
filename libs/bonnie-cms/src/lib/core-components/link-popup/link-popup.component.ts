import { Component, EventEmitter, Input, Output } from '@angular/core';

import { LocalizedString } from '@candy-kingdom/bonnie';
import { TextEditorField } from '../../core';

@Component({
  selector: 'bonc-link-popup',
  templateUrl: './link-popup.component.html',
  styleUrl: './link-popup.component.scss'
})
export class LinkPopupComponent {
  public readonly TextEditorField = TextEditorField;
  @Output()
  public linkChange: EventEmitter<LocalizedString> = new EventEmitter();

  @Output()
  public startEditing: EventEmitter<void> = new EventEmitter();

  @Output()
  public changed: EventEmitter<void> = new EventEmitter();

  @Output()
  public blurred: EventEmitter<void> = new EventEmitter();

  @Output()
  public open: EventEmitter<void> = new EventEmitter();

  @Output()
  public closed: EventEmitter<void> = new EventEmitter();

  @Input({ required: true })
  public field!: TextEditorField;
  public LinkPopupField = TextEditorField;

  @Input()
  public maxRows?: number;

  @Input()
  public minRows?: number;

  @Input({ required: true })
  public linkTitle!: LocalizedString; // todo: allready in weblink

  @Input({ required: true })
  public locale!: string;

  @Input({ required: true })
  public link!: LocalizedString;

  public popupIsShown = false;

  public onClick() {
    this.startEditing.emit();
  }

  public onChange() {
    console.log('change link pop up')
    this.changed.emit();
  }

  public onBlur() {
    this.blurred.emit();
  }

  public showPopup(): void {
    this.popupIsShown = true;
    this.open.emit();
  }

  public hidePopup(): void {
    this.popupIsShown = false;
    this.closed.emit();
  }
}
