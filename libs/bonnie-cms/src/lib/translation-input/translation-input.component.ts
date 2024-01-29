import { Component, EventEmitter, HostBinding, Input, Output } from '@angular/core';
import { DeviceType, TranslationInputStyle } from '../core';
import { LocalizedString } from '@candy-kingdom/bonnie';

@Component({
  selector: 'bonc-translation-input',
  templateUrl: './translation-input.component.html',
  styleUrls: ['./translation-input.component.scss']
})
export class TranslationInputComponent {
  public readonly EditorTextStyle = TranslationInputStyle;

  @Input()
  public style: TranslationInputStyle = TranslationInputStyle.NotSet;

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

  @HostBinding('class.form') get isFormStyle(): boolean {
    return this.style !== undefined && this.style === TranslationInputStyle.Form;
  }

  @HostBinding('class.big') get isBigStyle(): boolean {
    return this.style !== undefined && this.style === TranslationInputStyle.Big;
  }

  @HostBinding('class.small') get isSmallStyle(): boolean {
    return this.style !== undefined && this.style === TranslationInputStyle.Small;
  }

  @HostBinding('class.desktop') get isDesktop(): boolean {
    return this.device !== undefined && this.device === DeviceType.Desktop;
  }

  @HostBinding('class.tablet') get isTablet(): boolean {
    return this.device !== undefined && this.device === DeviceType.Tablet;
  }

  @HostBinding('class.mobile') get isMobile(): boolean {
    return this.device !== undefined && this.device === DeviceType.Mobile;
  }

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
