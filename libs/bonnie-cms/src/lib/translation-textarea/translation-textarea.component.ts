import {
  AfterViewInit,
  Component,
  EventEmitter,
  HostBinding,
  Input,
  NgZone,
  OnChanges,
  Output,
  QueryList,
  SimpleChanges,
  ViewChildren
} from '@angular/core';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';

import { LocalizedString } from '@candy-kingdom/bonnie';
import { DeviceType, TranslationInputStyle } from '../core';

@Component({
  selector: 'bonc-translation-textarea',
  templateUrl: './translation-textarea.component.html',
  styleUrls: ['./translation-textarea.component.scss']
})
export class TranslationTextareaComponent implements OnChanges, AfterViewInit {
  public readonly EditorTextStyle = TranslationInputStyle;

  @ViewChildren(CdkTextareaAutosize)
  public autosizeList!: QueryList<CdkTextareaAutosize>;

  @Input()
  public minRows?: number;

  @Input()
  public maxRows?: number;

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

  private _style: TranslationInputStyle = TranslationInputStyle.NotSet;

  @HostBinding('class.big') get isBigStyle(): boolean {
    return this.style !== undefined && this.style === TranslationInputStyle.Big;
  }

  @HostBinding('class.small') get isSmallStyle(): boolean {
    return this.style !== undefined && this.style === TranslationInputStyle.Small;
  }

  @HostBinding('class.form') get isFormStyle(): boolean {
    return this.style !== undefined && this.style === TranslationInputStyle.Form;
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

  constructor(
    private ngZone: NgZone
  ) {
  }

  ngAfterViewInit(): void {
    setTimeout(this.triggerResize.bind(this));
  }

  ngOnChanges(changes: SimpleChanges): void {
    setTimeout(this.triggerResize.bind(this), 500);
  }

  @Input()
  public set style(style: TranslationInputStyle) {
    this._style = style;
    this.triggerResize();
  }

  public get style(): TranslationInputStyle {
    return this._style;
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

  private triggerResize() {
    // console.log('trigger resize!');

    this.ngZone.onStable.pipe(take(1))
      .subscribe(() => {
        this.autosizeList.forEach(x => x.resizeToFitContent(true));
      });
  }
}
