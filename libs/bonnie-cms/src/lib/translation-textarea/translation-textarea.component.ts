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
import { DeviceType } from '../core';

@Component({
  selector: 'bonc-translation-textarea',
  templateUrl: './translation-textarea.component.html',
  styleUrls: ['./translation-textarea.component.scss']
})
export class TranslationTextareaComponent implements OnChanges, AfterViewInit {
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

  constructor(
    private ngZone: NgZone
  ) {
  }

  ngAfterViewInit(): void {
    setTimeout(this.triggerResize.bind(this));
  }

  ngOnChanges(): void {
    setTimeout(this.triggerResize.bind(this), 500);
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
