import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DeviceType } from '../core';
import { LocalizedString } from '@candy-kingdom/bonnie';

@Component({
  selector: 'bonc-translation-input',
  standalone: true,
  templateUrl: './translation-input.component.html',
  styleUrls: ['./translation-input.component.scss']
})
export class TranslationInputComponent {
  @Input({ required: true })
  public text!: LocalizedString;

  @Input({ required: true })
  public locale!: string;

  @Input()
  public device: DeviceType = DeviceType.NotSet;

  @Output()
  public startEditing: EventEmitter<void> = new EventEmitter();

  @Output()
  public changed: EventEmitter<void> = new EventEmitter();

  @Output()
  public blurred: EventEmitter<void> = new EventEmitter();

  public onClick() {
    this.startEditing.emit();
  }

  public onKeyPress() {
    this.changed.emit();
  }

  public onBlur() {
    this.blurred.emit();
  }
}
