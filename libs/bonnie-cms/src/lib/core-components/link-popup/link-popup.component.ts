import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TranslationInputComponent } from '../../translation-input/translation-input.component';
import { TranslationTextareaComponent } from '../../translation-textarea/translation-textarea.component';

import { LocalizedString } from '@candy-kingdom/bonnie';
import { TextEditorField } from '../../core';

@Component({
  selector: 'bonc-link-popup',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslationInputComponent, TranslationTextareaComponent],
  templateUrl: './link-popup.component.html',
  styleUrl: './link-popup.component.scss'
})
export class LinkPopupComponent {
  public readonly TextEditorField = TextEditorField;
  public readonly LinkPopupField = TextEditorField;

  public readonly linkChange = output<LocalizedString>();

  public readonly startEditing = output<void>();

  public readonly changed = output<void>();

  public readonly blurred = output<void>();

  public readonly open = output<void>();

  public readonly closed = output<void>();

  public readonly field = input.required<TextEditorField>();

  public readonly maxRows = input<number>();

  public readonly minRows = input<number>();

  public readonly linkTitle = input.required<LocalizedString>();

  public readonly locale = input.required<string>();

  public readonly link = input.required<LocalizedString>();

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
