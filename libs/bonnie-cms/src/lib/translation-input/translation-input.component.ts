import { Component, input, output } from '@angular/core';
import { DeviceType } from '../core';
import { LocalizedString } from '@candy-kingdom/bonnie';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'bonc-translation-input',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './translation-input.component.html',
  styleUrls: ['./translation-input.component.scss']
})
export class TranslationInputComponent {
  public readonly text = input.required<LocalizedString>();

  public readonly locale = input.required<string>();

  public readonly device = input<DeviceType>(DeviceType.NotSet);

  public readonly startEditing = output<void>();

  public readonly changed = output<void>();

  public readonly blurred = output<void>();

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
